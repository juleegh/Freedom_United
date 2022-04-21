using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectMarker : SetOfPositionsMarker
{

}

public class SetOfPositionsMarker : MonoBehaviour
{
    [SerializeField] private Transform container;

    public SetOfPositions GetPositions()
    {
        Vector3Int roundedPos = Vector3Int.RoundToInt(transform.position);
        Vector2Int center = new Vector2Int(roundedPos.x, roundedPos.z);

        List<Vector2Int> positions = new List<Vector2Int>();

        PositionMarker[] cells = container.GetComponentsInChildren<PositionMarker>();
        foreach (PositionMarker cell in cells)
        {
            positions.Add(cell.GetPosition() - center);
        }

        return new SetOfPositions(positions);
    }

    [ContextMenu("Toggle On")]
    public void ToggleOn()
    {
        PositionMarker[] cells = container.GetComponentsInChildren<PositionMarker>();
        foreach (PositionMarker cell in cells)
        {
            cell.Toggle(true);
        }
    }

    [ContextMenu("Toggle Off")]
    public void ToggleOff()
    {
        PositionMarker[] cells = container.GetComponentsInChildren<PositionMarker>();
        foreach (PositionMarker cell in cells)
        {
            cell.Toggle(false);
        }
    }
}
