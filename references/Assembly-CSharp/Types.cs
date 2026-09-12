using System.Collections.Generic;
using System;
using UnityEngine;

public class DysonSphereLayer
{
    private DysonSphereLayer() { }
    public int id;
    public float orbitRadius;
    public DysonNode[] nodePool;
    public DysonFrame[] framePool;
    public DysonShell[] shellPool;
    public StarData starData;
    public int NewDysonNode(int prototype, Vector3 position) => throw new NotSupportedException();
    public int NewDysonFrame(int prototype, int a, int b, bool euler) => throw new NotSupportedException();
}

public class DysonNode
{
    private DysonNode() { }
    public int id, protoId, sp, spMax;
    public Vector3 pos;
    public Color32 color;
    public List<DysonFrame> frames;
    public List<DysonShell> shells;
}

public class DysonFrame
{
    private DysonFrame() { }
    public int id, protoId, spA, spB, spMax;
    public bool euler;
    public Color32 color;
    public DysonNode nodeA, nodeB;
}

public class DysonShell
{
    private DysonShell() { }
    public int id, protoId;
    public Color32 color;
    public List<DysonNode> nodes;
    public List<DysonFrame> frames;
    public int[] nodecps;
}

public abstract class ManualBehaviour : MonoBehaviour
{
    protected ManualBehaviour() => throw new NotSupportedException();
    public bool active => throw new NotSupportedException();
}
public class UIRoot : ManualBehaviour
{
    private UIRoot() { }
    public static UIRoot instance => throw new NotSupportedException();
    public UIGame uiGame;
}
public class UIGame : ManualBehaviour
{
    private UIGame() { }
    public UIDysonEditor dysonEditor;
}
public class UIDysonEditor : ManualBehaviour
{
    private UIDysonEditor() { }
    public DESelection selection;
    public UIDEControlPanel controlPanel;
    public RectTransform[] guiRects;
}
public class UIDEControlPanel : ManualBehaviour
{
    private UIDEControlPanel() { }
    public UIDEToolbox toolbox;
}
public class UIDEToolbox : ManualBehaviour
{
    private UIDEToolbox() { }
    public RectTransform selfRect;
}
public class UIBlockZone : MonoBehaviour { private UIBlockZone() { } }
public class DESelection
{
    private DESelection() { }
    public DysonSphereLayer singleSelectedLayer => throw new NotSupportedException();
}
public class GameMain : MonoBehaviour
{
    private GameMain() { }
    public static bool isRunning => throw new NotSupportedException();
    public static GameHistoryData history => throw new NotSupportedException();
}
public class GameHistoryData
{
    private GameHistoryData() { }
    public float dysonNodeLatitude;
}
public class StarData
{
    private StarData() { }
    public int id;
}
