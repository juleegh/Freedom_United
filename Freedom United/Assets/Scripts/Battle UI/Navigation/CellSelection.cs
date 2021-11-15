using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSelection
{
    private int MaxRow { get { return BattleManager.Instance.BattleGrid.Height; } }
    private int MaxColumn { get { return BattleManager.Instance.BattleGrid.Width; } }
    private int currentRow;
    private int currentColumn;

    public void Initialize(Vector2Int startPosition)
    {
        currentColumn = startPosition.x;
        currentRow = startPosition.y;
        BattleUIManager.Instance.CellSelectionUI.UpdateSelection(currentColumn, currentRow);
    }

    public void Down()
    {
        if (currentRow == 0)
            return;

        currentRow--;
        BattleUIManager.Instance.CellSelectionUI.UpdateSelection(currentColumn, currentRow);
    }

    public void Up()
    {
        if (currentRow == MaxRow - 1)
            return;

        currentRow++;
        BattleUIManager.Instance.CellSelectionUI.UpdateSelection(currentColumn, currentRow);
    }

    public void Left()
    {
        if (currentColumn == 0)
            return;

        currentColumn--;
        BattleUIManager.Instance.CellSelectionUI.UpdateSelection(currentColumn, currentRow);
    }

    public void Right()
    {
        if (currentColumn == MaxColumn - 1)
            return;

        currentColumn++;
        BattleUIManager.Instance.CellSelectionUI.UpdateSelection(currentColumn, currentRow);
    }

    public void Toggle(bool visible)
    {
        BattleUIManager.Instance.CellSelectionUI.Toggle(visible);
    }

    public Vector2Int SelectedPosition { get { return new Vector2Int(currentColumn, currentRow); } }
}