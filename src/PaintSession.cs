using System;
using System.Linq;

namespace DSPSphereBuilder
{
    internal interface IPatchLayer
    {
        int StarId { get; }
        int LayerId { get; }
        float Radius { get; }
        LayerGraph Capture();
        int AddNode(Position position);
        int AddFrame(int a, int b);
    }

    internal sealed class PaintTarget
    {
        public readonly IPatchLayer Layer;
        public readonly int RoundedLatitude;
        public PaintTarget(IPatchLayer layer, int roundedLatitude) { Layer = layer; RoundedLatitude = roundedLatitude; }
    }

    internal enum PaintState { Unavailable, LatitudeRequired, InvalidPosition, Ready, Applied, Complete, Mismatch, Stopped }

    internal sealed class PaintResult
    {
        public PaintState State;
        public int StarId, LayerId, CompletedPatches, NodeCount, FrameCount;
        public float Radius;
        public bool CanPaint => State == PaintState.Ready || State == PaintState.Applied;
    }

    internal sealed class PaintSession
    {
        private readonly string targetIdentity;
        private readonly Action<string> logError;
        public bool Stopped { get; private set; }

        public PaintSession(string targetIdentity, Action<string> logError)
        {
            this.targetIdentity = targetIdentity;
            this.logError = logError;
        }

        public PaintResult Describe(Func<PaintTarget> resolve) => Run(resolve, false);
        public PaintResult Paint(Func<PaintTarget> resolve) => Run(resolve, true);

        public void Stop(Exception error)
        {
            if (Stopped) return;
            Stopped = true;
            logError($"Build {BuildInfo.Label}; target {targetIdentity}; editor integration failed: {error}");
        }

        private PaintResult Run(Func<PaintTarget> resolve, bool paint)
        {
            var result = new PaintResult();
            if (Stopped) { result.State = PaintState.Stopped; return result; }
            int intendedPatch = 0;
            try
            {
                var target = resolve();
                if (target == null) return result;
                var layer = target.Layer;
                result.StarId = layer.StarId; result.LayerId = layer.LayerId; result.Radius = layer.Radius;
                if (target.RoundedLatitude < 68) { result.State = PaintState.LatitudeRequired; return result; }
                var before = layer.Capture();
                var match = GraphRecognition.Match(before);
                result.CompletedPatches = match.CompletedPatches;
                result.NodeCount = before.Nodes.Length; result.FrameCount = before.Frames.Length;
                if (match.State == GraphState.Mismatch) { result.State = PaintState.Mismatch; return result; }
                if (match.State == GraphState.Complete) { result.State = PaintState.Complete; return result; }
                var patch = SpherePlan.Patches[match.CompletedPatches];
                var positions = patch.Nodes.ToDictionary(id => id, id => SpherePlan.Position(id, before.Radius, match.Orientation));
                if (positions.Values.Any(p => !p.IsFinite)) { result.State = PaintState.InvalidPosition; return result; }
                result.State = PaintState.Ready;
                if (!paint) return result;

                intendedPatch = match.CompletedPatches + 1;
                var ids = match.NativeIds;
                foreach (int id in patch.Nodes)
                {
                    int nativeId = layer.AddNode(positions[id]);
                    if (nativeId <= 0) throw new InvalidOperationException("Native node creation returned an invalid ID.");
                    ids.Add(id, nativeId);
                }
                foreach (var edge in patch.Frames)
                    if (layer.AddFrame(ids[edge.A], ids[edge.B]) == 0)
                        throw new InvalidOperationException("Native frame creation returned zero.");

                var after = layer.Capture();
                Preservation.Check(before, after);
                var next = GraphRecognition.Match(after);
                if (next.State == GraphState.Mismatch || next.CompletedPatches != intendedPatch || next.Orientation != match.Orientation)
                    throw new InvalidOperationException("Result does not match the intended next patch.");
                result.CompletedPatches = next.CompletedPatches;
                result.NodeCount = after.Nodes.Length; result.FrameCount = after.Frames.Length;
                result.State = next.State == GraphState.Complete ? PaintState.Complete : PaintState.Applied;
                return result;
            }
            catch (Exception error)
            {
                // Earlier native writes can survive the failing call; do not replay the delta.
                Stopped = true;
                result.State = PaintState.Stopped;
                logError($"Build {BuildInfo.Label}; target {targetIdentity}; star {result.StarId}, layer {result.LayerId}, radius {result.Radius}, patch {intendedPatch}: {error}");
                return result;
            }
        }
    }
}
