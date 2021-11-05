using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer background;

    public void Refresh(CellType cellType)
    {
        background.color = cellType == CellType.Available ? Color.gray : Color.black;
    }
}
