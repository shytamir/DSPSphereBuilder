using System;

namespace DSPSphereBuilder
{
    internal static class Feedback
    {
        public static string Text(PaintResult result)
        {
            switch (result.State)
            {
                case PaintState.Unavailable: return "Select exactly one layer in a running game.";
                case PaintState.LatitudeRequired: return "Requires 68 degrees of Dyson sphere latitude.";
                case PaintState.InvalidPosition: return "Cannot place this patch at the layer's radius.";
                case PaintState.Mismatch: return "This layer does not match the staged design.";
                case PaintState.Stopped: return "Sphere Builder stopped after an error.\nA partial patch may remain; see the game log.";
                case PaintState.Complete: return $"Layer {result.LayerId}: framework complete (12/12).";
                case PaintState.Ready when result.CompletedPatches == 0: return $"Layer {result.LayerId}: ready for patch 1 of 12.";
                case PaintState.Ready:
                case PaintState.Applied: return $"Layer {result.LayerId}: {result.CompletedPatches}/12 patches planned.";
                default: throw new ArgumentOutOfRangeException(nameof(result.State));
            }
        }
    }
}
