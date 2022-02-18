using UnityEngine;
using System.Collections.Generic;

public class BossPart
{
    private BossPartType partType;
    private Vector2Int position;
    private int width;
    private int height;
    private Vector2Int orientation;
    private bool shouldRotate;

    public BossPartType PartType { get { return partType; } }
    public Vector2Int Position { get { return position; } }
    public int Width { get { return width; } }
    public int Height { get { return height; } }
    public bool ShouldRotate { get { return shouldRotate; } }
    public Vector2Int Orientation { get { return orientation; } }

    public BossPart(BossPartType partT, Vector2Int p, int w, int h, bool rotates)
    {
        partType = partT;
        position = p;
        width = w;
        height = h;
        shouldRotate = rotates;
        orientation = Vector2Int.down;
    }

    public bool OccupiesPosition(int x, int y)
    {
        return x >= position.x - width / 2 && x <= position.x + width / 2 &&
        y >= position.y - height / 2 && y <= position.y + height / 2;
    }

    public List<Vector2Int> GetOccupiedPositions()
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        Vector2Int verticaldelta = -orientation;
        Vector2Int horizontaldelta = new Vector2Int(-orientation.y, orientation.x);

        for (int column = 0; column < width; column++)
        {
            for (int row = 0; row < height; row++)
            {
                Vector2Int delta = horizontaldelta * column + verticaldelta * row;
                positions.Add(delta + position);
            }
        }

        return positions;
    }

    public void Rotate(Vector2Int pivot, Vector2Int newOrientation)
    {
        Vector2Int center = GetCenterPosition();
        orientation = newOrientation;
        Vector2Int orientedTransformation = center + GetOrientedTransformation(width / 2, height / 2);
        Vector2Int distanceFromPivot = center - pivot;
        Vector2Int pivotTransformation = new Vector2Int(distanceFromPivot.y - distanceFromPivot.x, -(distanceFromPivot.x + distanceFromPivot.y));
        position = orientedTransformation + pivotTransformation;
    }

    public Vector2Int GetCenterPosition()
    {
        Vector2Int transformation = GetOrientedTransformation(width / 2, height / 2);
        return new Vector2Int(position.x - transformation.x, position.y - transformation.y);
    }

    public Vector2Int GetWorldPosition()
    {
        return GetCenterPosition() + GetOrientedTransformation(width / 2, height / 2);
    }

    private Vector2Int GetOrientedTransformation(int x, int y)
    {
        return BossUtils.GetOrientedTransformation(orientation, x, y);
    }

    public override string ToString()
    {
        return partType.ToString();
    }
}