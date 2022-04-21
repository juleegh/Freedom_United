using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[Serializable]
public class SetOfPositions
{
    [SerializeField] private List<Vector2Int> positions;
    public SetOfPositions(List<Vector2Int> pos) { positions = pos; }
    public List<Vector2Int> Positions { get { return positions; } }

    public bool PositionInsideArea(Vector2Int targetPosition, Vector2Int pivotPosition, Vector2Int orientation)
    {
        foreach (Vector2Int pos in positions)
        {
            Vector2Int affectedPosition = BossUtils.GetOrientedTransformation(orientation, pos.x, pos.y) + pivotPosition;
            if (affectedPosition == targetPosition)
                return true;
        }
        return false;
    }

    public List<Vector2Int> GetRotatedDeltas(Vector2Int orientation)
    {
        List<Vector2Int> transformed = new List<Vector2Int>();
        foreach (Vector2Int pos in positions)
        {
            Vector2Int affectedPosition = BossUtils.GetOrientedTransformation(orientation, pos.x, pos.y);
            transformed.Add(affectedPosition);
        }
        return transformed;
    }

    public List<Vector2Int> GetRotatedDeltasWithPivot(Vector2Int pivot, Vector2Int orientation)
    {
        List<Vector2Int> transformed = new List<Vector2Int>();
        foreach (Vector2Int pos in positions)
        {
            Vector2Int affectedPosition = BossUtils.GetOrientedTransformation(orientation, pos.x, pos.y);
            transformed.Add(affectedPosition + pivot);
        }
        return transformed;
    }

    public List<Vector2Int> GetPositions(Vector2Int pivot, Vector2Int orientation)
    {
        List<Vector2Int> transformed = new List<Vector2Int>();
        foreach (Vector2Int pos in positions)
        {
            Vector2Int affectedPosition = pivot + BossUtils.GetOrientedTransformation(orientation, pos.x, pos.y);
            transformed.Add(affectedPosition);
        }
        return transformed;
    }
}