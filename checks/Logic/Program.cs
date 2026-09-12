using DSPSphereBuilder;
using System.Text.Json;

if (args.Length != 1) throw new ArgumentException("Output path for compiled plan comparison is required.");
var export = new
{
    reference = SpherePlan.ReferenceSha256,
    directions = SpherePlan.Directions.Select(p => new[] { p.X, p.Y, p.Z }),
    patches = SpherePlan.Patches.Select(p => new { nodes = p.Nodes, frames = p.Frames.Select(e => new[] { e.A, e.B }) }),
    faces = SpherePlan.Faces,
    scales = new[] { 100f, 9700f, 36000f, 100000f, 1000000f }.Select(radius => new
    {
        radius,
        positions = Enumerable.Range(1, 60).Select(id => SpherePlan.Position(id, radius)).Select(p => new[] { p.X, p.Y, p.Z })
    })
};
File.WriteAllText(args[0], JsonSerializer.Serialize(export));
Console.WriteLine("Exported compiled production plan for independent geometry comparison.");
