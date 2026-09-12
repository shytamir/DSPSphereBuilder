using UnityEngine;

namespace DSPSphereBuilder
{
    internal sealed class NativeTarget : IPatchLayer
    {
        private readonly DysonSphereLayer layer;
        private NativeTarget(DysonSphereLayer layer) { this.layer = layer; }
        public int StarId => layer.starData.id;
        public int LayerId => layer.id;
        public float Radius => layer.orbitRadius;
        public LayerGraph Capture() => NativeGraph.Capture(layer);
        public int AddNode(Position p) => layer.NewDysonNode(0, new Vector3(p.X, p.Y, p.Z));
        public int AddFrame(int a, int b) => layer.NewDysonFrame(0, a, b, false);

        public static UIDysonEditor Editor => UIRoot.instance?.uiGame?.dysonEditor;
        public static PaintTarget Resolve()
        {
            var editor = Editor;
            if (editor == null || !editor.active || !GameMain.isRunning) return null;
            var selected = editor.selection?.singleSelectedLayer;
            return selected == null ? null : new PaintTarget(new NativeTarget(selected), Mathf.RoundToInt(GameMain.history.dysonNodeLatitude));
        }
    }
}
