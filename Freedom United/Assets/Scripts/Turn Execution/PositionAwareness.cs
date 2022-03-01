using UnityEngine;

public class PositionAwareness
{
    public int turnsInMemory;
    public Vector2Int position;

    public PositionAwareness(Vector2Int currentPosition)
    {
        position = currentPosition;
        turnsInMemory = 2;
    }

    public void Reset(Vector2Int currentPosition)
    {
        position = currentPosition;
        turnsInMemory = 2;
    }
}