using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOrganizer : MonoBehaviour
{
    private void Awake()
    {
        Organize();
    }

    [ContextMenu("Organize")]
    public void Organize()
    {
        GridElement[] cells = GetComponentsInChildren<GridElement>();
        foreach (GridElement cell in cells)
        {
            Vector3Int roundedPos = Vector3Int.RoundToInt(cell.transform.position);
            roundedPos.y = 0;
            cell.transform.position = roundedPos;
        }
    }
}
