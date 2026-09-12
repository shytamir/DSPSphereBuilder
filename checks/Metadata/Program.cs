using System.Collections.Immutable;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

if (args.Length is < 4 or > 5) throw new ArgumentException("DLL, package version, build label, assembly version, and optional native-reference DLL are required.");
var built = Inspect(args[0], args[1], args[2], args[3]);
if (args.Length == 5)
{
    var native = Inspect(args[4], args[1], args[2], args[3]);
    var differences = built.Except(native).Select(s => "Only shim build: " + s)
        .Concat(native.Except(built).Select(s => "Only native build: " + s)).ToArray();
    if (differences.Length != 0) throw new InvalidDataException(string.Join(Environment.NewLine, differences));
    Console.WriteLine("PASS: emitted assembly/member references match real-reference compilation.");
}

static HashSet<string> Inspect(string path, string version, string label, string assemblyVersion)
{
    using var stream = File.OpenRead(path);
    using var pe = new PEReader(stream);
    var reader = pe.GetMetadataReader();
    var names = new Names();
    var assembly = reader.GetAssemblyDefinition();
    if (reader.GetString(assembly.Name) != "DSPSphereBuilder" || assembly.Version.ToString() != assemblyVersion)
        throw new InvalidDataException("Unexpected assembly identity.");
    string? informational = null;
    bool plugin = false;
    foreach (var handle in reader.CustomAttributes)
    {
        var attribute = reader.GetCustomAttribute(handle);
        if (attribute.Constructor.Kind != HandleKind.MemberReference) continue;
        var constructor = reader.GetMemberReference((MemberReferenceHandle)attribute.Constructor);
        var type = names.Entity(reader, constructor.Parent);
        var value = reader.GetBlobReader(attribute.Value);
        if (value.ReadUInt16() != 1) throw new InvalidDataException("Invalid attribute prolog.");
        if (type.EndsWith("::System.Reflection.AssemblyInformationalVersionAttribute")) informational = value.ReadSerializedString();
        if (type.EndsWith("::System.Reflection.AssemblyMetadataAttribute") && value.ReadSerializedString() == "ReferenceShim")
            throw new InvalidDataException("Reference shim cannot be the plugin payload.");
        if (type == "BepInEx::BepInEx.BepInPlugin")
        {
            if (value.ReadSerializedString() != "dsp.spherebuilder" || value.ReadSerializedString() != "DSP Sphere Builder" || value.ReadSerializedString() != version)
                throw new InvalidDataException("Plugin GUID/name/version differs from package identity.");
            plugin = true;
        }
    }
    if (!plugin || informational != label) throw new InvalidDataException("Missing plugin identity or incorrect diagnostic revision.");
    var references = new HashSet<string>();
    foreach (var handle in reader.AssemblyReferences)
    {
        var reference = reader.GetAssemblyReference(handle);
        references.Add($"assembly {reader.GetString(reference.Name)} {reference.Version} {Convert.ToHexString(reader.GetBlobBytes(reference.PublicKeyOrToken))}");
    }
    foreach (var handle in reader.MemberReferences)
    {
        var member = reader.GetMemberReference(handle);
        string signature;
        if (member.GetKind() == MemberReferenceKind.Field) signature = "field " + member.DecodeFieldSignature(names, (object?)null);
        else
        {
            var method = member.DecodeMethodSignature(names, (object?)null);
            signature = $"method {method.Header.IsInstance} {method.GenericParameterCount} {method.ReturnType} ({string.Join(",", method.ParameterTypes)})";
        }
        references.Add($"{names.Entity(reader, member.Parent)}::{reader.GetString(member.Name)} {signature}");
    }
    Console.WriteLine($"PASS: {Path.GetFileName(path)} / dsp.spherebuilder / {version} / {label}; {references.Count} emitted references inspected without loading the DLL.");
    return references;
}

sealed class Names : ISignatureTypeProvider<string, object?>
{
    public string Entity(MetadataReader reader, EntityHandle handle) => handle.Kind switch
    {
        HandleKind.TypeReference => GetTypeFromReference(reader, (TypeReferenceHandle)handle, 0),
        HandleKind.TypeDefinition => GetTypeFromDefinition(reader, (TypeDefinitionHandle)handle, 0),
        HandleKind.TypeSpecification => GetTypeFromSpecification(reader, null, (TypeSpecificationHandle)handle, 0),
        _ => handle.Kind.ToString()
    };
    public string GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
    {
        var type = reader.GetTypeReference(handle);
        var scope = type.ResolutionScope.Kind == HandleKind.AssemblyReference
            ? reader.GetString(reader.GetAssemblyReference((AssemblyReferenceHandle)type.ResolutionScope).Name)
            : Entity(reader, type.ResolutionScope);
        return scope + "::" + reader.GetString(type.Namespace) + "." + reader.GetString(type.Name);
    }
    public string GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
    {
        var type = reader.GetTypeDefinition(handle);
        return "local::" + reader.GetString(type.Namespace) + "." + reader.GetString(type.Name);
    }
    public string GetTypeFromSpecification(MetadataReader reader, object? context, TypeSpecificationHandle handle, byte kind) => reader.GetTypeSpecification(handle).DecodeSignature(this, context);
    public string GetArrayType(string element, ArrayShape shape) => element + "[" + new string(',', shape.Rank - 1) + "]";
    public string GetSZArrayType(string element) => element + "[]";
    public string GetByReferenceType(string element) => element + "&";
    public string GetPointerType(string element) => element + "*";
    public string GetPinnedType(string element) => "pinned " + element;
    public string GetPrimitiveType(PrimitiveTypeCode code) => code.ToString();
    public string GetGenericInstantiation(string genericType, ImmutableArray<string> args) => genericType + "<" + string.Join(",", args) + ">";
    public string GetGenericMethodParameter(object? context, int index) => "!!" + index;
    public string GetGenericTypeParameter(object? context, int index) => "!" + index;
    public string GetModifiedType(string modifier, string type, bool required) => $"{type} mod({required},{modifier})";
    public string GetFunctionPointerType(MethodSignature<string> signature) => signature.ReturnType + "*(" + string.Join(",", signature.ParameterTypes) + ")";
}
