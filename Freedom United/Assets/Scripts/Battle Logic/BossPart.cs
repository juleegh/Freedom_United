using UnityEngine;

public class BossPart
{
    private BossPartType partType;
    private Vector2Int position;
    private int width;
    private int height;

    public BossPartType PartType { get { return partType; } }
    public Vector2Int Position { get { return position; } }
    public int Width { get { return width; } }
    public int Height { get { return height; } }

    public BossPart(BossPartType partT, Vector2Int p, int w, int h)
    {
        partType = partT;
        position = p;
        width = w;
        height = h;
    }

    public bool OccupiesPosition(int x, int y)
    {
        return x >= position.x && x <= position.x + width &&
        y >= position.y && y <= position.y + height;
    }
}