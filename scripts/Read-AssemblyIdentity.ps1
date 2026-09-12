#Requires -Version 7.0
[CmdletBinding()]
param([Parameter(Mandatory)][string[]]$Path)

$ErrorActionPreference = 'Stop'
foreach ($inputPath in $Path) {
    $file = Get-Item -LiteralPath $inputPath
    $stream = [IO.File]::OpenRead($file.FullName)
    $pe = [Reflection.PortableExecutable.PEReader]::new($stream)
    try {
        $reader = [Reflection.Metadata.PEReaderExtensions]::GetMetadataReader($pe)
        $assembly = $reader.GetAssemblyDefinition()
        $module = $reader.GetModuleDefinition()
        $references = foreach ($handle in $reader.AssemblyReferences) {
            $reference = $reader.GetAssemblyReference($handle)
            [pscustomobject]@{
                name = $reader.GetString($reference.Name)
                version = $reference.Version.ToString()
            }
        }
        [pscustomobject]@{
            file = $file.Name
            size = $file.Length
            sha256 = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash
            name = $reader.GetString($assembly.Name)
            version = $assembly.Version.ToString()
            mvid = $reader.GetGuid($module.Mvid).ToString()
            references = @($references)
        }
    }
    finally { $pe.Dispose(); $stream.Dispose() }
}
