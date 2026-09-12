using System.Collections.Generic;
using UnityEngine;

public class DysonSphereLayer
{
    private DysonSphereLayer() { }
    public int id;
    public float orbitRadius;
    public DysonNode[] nodePool;
    public DysonFrame[] framePool;
    public DysonShell[] shellPool;
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
