using DSPSphereBuilder;
using System.Text.Json;

if (args.Length != 1) throw new ArgumentException("Output path for compiled plan comparison is required.");
RecognitionChecks.Run();
PaintChecks.Run();
var export = new
{
    reference = SpherePlan.ReferenceSha256,
    patches = SpherePlan.Patches.Select(p => new { nodes = p.Nodes, frames = p.Frames.Select(e => new[] { e.A, e.B }) }),
    faces = SpherePlan.Faces,
    plans = new[] { PlanOrientation.Grid, PlanOrientation.Legacy }.Select(orientation => new
    {
        orientation = orientation.ToString(),
        directions = (orientation == PlanOrientation.Legacy ? SpherePlan.LegacyDirections : SpherePlan.Directions)
            .Select(p => new[] { p.X, p.Y, p.Z }),
        scales = new[] { 100f, 9700f, 36000f, 100000f, 1000000f }.Select(radius => new
        {
            radius,
            positions = Enumerable.Range(1, 60).Select(id => SpherePlan.Position(id, radius, orientation))
                .Select(p => new[] { p.X, p.Y, p.Z })
        })
    })
};
File.WriteAllText(args[0], JsonSerializer.Serialize(export));
Console.WriteLine("Exported compiled production plan for independent geometry comparison.");
