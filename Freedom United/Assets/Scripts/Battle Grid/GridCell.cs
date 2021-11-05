using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    private CellType cellType;
    public CellType CellType { get { return cellType; } }

    public GridCell(CellType type)
    {
        cellType = type;
    }
}
