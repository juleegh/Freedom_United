using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSelection
{
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
        if (!BattleManager.Instance.BattleGrid.IsInsideGrid(currentColumn, currentRow - 1))
            return;

        currentRow--;
        BattleUINavigation.Instance.NavigationState.currentAction.position = SelectedPosition;
        BattleUIManager.Instance.CellSelectionUI.UpdateSelection(currentColumn, currentRow);
    }

    public void Up()
    {
        if (!BattleManager.Instance.BattleGrid.IsInsideGrid(currentColumn, currentRow + 1))
            return;

        currentRow++;
        BattleUINavigation.Instance.NavigationState.currentAction.position = SelectedPosition;
        BattleUIManager.Instance.CellSelectionUI.UpdateSelection(currentColumn, currentRow);
    }

    public void Left()
    {
        if (!BattleManager.Instance.BattleGrid.IsInsideGrid(currentColumn - 1, currentRow))
            return;

        currentColumn--;
        BattleUINavigation.Instance.NavigationState.currentAction.position = SelectedPosition;
        BattleUIManager.Instance.CellSelectionUI.UpdateSelection(currentColumn, currentRow);
    }

    public void Right()
    {
        if (!BattleManager.Instance.BattleGrid.IsInsideGrid(currentColumn + 1, currentRow))
            return;

        currentColumn++;
        BattleUINavigation.Instance.NavigationState.currentAction.position = SelectedPosition;
        BattleUIManager.Instance.CellSelectionUI.UpdateSelection(currentColumn, currentRow);
    }

    public void Toggle(bool visible)
    {
        BattleUIManager.Instance.CellSelectionUI.Toggle(visible);
    }

    public Vector2Int SelectedPosition { get { return new Vector2Int(currentColumn, currentRow); } }
}