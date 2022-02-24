using UnityEngine;
using System.Collections.Generic;

public class BossPart
{
    private BossPartConfig partStats;
    private Vector2Int position;
    private Vector2Int orientation;

    public BossPartType PartType { get { return partStats.PartType; } }
    public Vector2Int Position { get { return position; } }
    public int Width { get { return partStats.Dimensions.x; } }
    public int Height { get { return partStats.Dimensions.y; } }
    public bool ShouldRotate { get { return partStats.RotatesWithBody; } }
    public Vector2Int Orientation { get { return orientation; } }

    public BossPart(BossPartConfig partStat)
    {
        partStats = partStat;
        position = partStat.InitialPosition;
        orientation = Vector2Int.down;
    }

    public bool OccupiesPosition(int x, int y)
    {
        Vector2Int verticaldelta = -orientation;
        Vector2Int horizontaldelta = new Vector2Int(-orientation.y, orientation.x);
        Vector2Int oppositeCorner = horizontaldelta * (Width - 1) + verticaldelta * (Height - 1) + position;

        return x >= Mathf.Min(position.x, oppositeCorner.x) && x <= Mathf.Max(position.x, oppositeCorner.x) &&
            y >= Mathf.Min(position.y, oppositeCorner.y) && y <= Mathf.Max(position.y, oppositeCorner.y);
    }

    public List<Vector2Int> GetOccupiedPositions()
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        Vector2Int verticaldelta = -orientation;
        Vector2Int horizontaldelta = new Vector2Int(-orientation.y, orientation.x);

        for (int column = 0; column < Width; column++)
        {
            for (int row = 0; row < Height; row++)
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
        Vector2Int orientedTransformation = center + GetOrientedTransformation(Width / 2, Height / 2);
        Vector2Int distanceFromPivot = center - pivot;
        Vector2Int pivotTransformation = new Vector2Int(distanceFromPivot.y - distanceFromPivot.x, -(distanceFromPivot.x + distanceFromPivot.y));
        position = orientedTransformation + pivotTransformation;
    }

    public Vector2Int GetCenterPosition()
    {
        Vector2Int transformation = GetOrientedTransformation(Width / 2, Height / 2);
        return new Vector2Int(position.x - transformation.x, position.y - transformation.y);
    }

    public Vector2Int GetWorldPosition()
    {
        return GetCenterPosition() + GetOrientedTransformation(Width / 2, Height / 2);
    }

    private Vector2Int GetOrientedTransformation(int x, int y)
    {
        return BossUtils.GetOrientedTransformation(orientation, x, y);
    }

    private List<Vector2Int> GetDeltaOfActionForPosition(SetOfPositions usedSet, Vector2Int actionCenter)
    {
        List<Vector2Int> transformedSet = usedSet.GetRotatedDeltas(orientation);
        Vector2Int distanceApplied = actionCenter - transformedSet[0];

        List<Vector2Int> resultDelta = new List<Vector2Int>();
        foreach (Vector2Int transformed in transformedSet)
        {
            resultDelta.Add(transformed + distanceApplied);
        }

        return resultDelta;
    }

    public override string ToString()
    {
        return PartType.ToString();
    }
}