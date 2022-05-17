using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPileSelection : NavigationSelection
{
    protected override int MaxElements { get { return BattleManager.Instance.ActionPile.ActionsForTurn.Count; } }
    private int topElement;
    private int ElementsOnScreen { get { return BattleUIManager.Instance.ActionPileUI.ActionsOnScreen; } }
    private bool focus { get { return BattleUINavigation.Instance.CurrentLevel == BattleSelectionLevel.ActionPile; } }

    public override void Next()
    {
        if (currentIndex + topElement == MaxElements - 1)
            return;

        if (currentIndex < ElementsOnScreen - 1 && currentIndex < MaxElements - 1)
        {
            currentIndex++;
            BattleUIManager.Instance.ActionPileUI.RefreshView(topElement, currentIndex);
        }
        else if (topElement + ElementsOnScreen + 1 <= MaxElements)
        {
            topElement++;
            BattleUIManager.Instance.ActionPileUI.RefreshView(topElement, currentIndex);
        }
        PaintPreviewRange();
    }

    public override void Previous()
    {
        if (currentIndex + topElement == 0)
            return;

        if (currentIndex > 0)
        {
            currentIndex--;
            BattleUIManager.Instance.ActionPileUI.RefreshView(topElement, currentIndex);
        }
        else if (topElement > 0)
        {
            topElement--;
            BattleUIManager.Instance.ActionPileUI.RefreshView(topElement, currentIndex);
        }
        PaintPreviewRange();
    }

    private void PaintPreviewRange()
    {
        BattleActionType actionType = ShowingAction.actionType;
        if (ShowingAction as AllyAction != null)
        {
            AllyAction allyAction = ShowingAction as AllyAction;
            CharacterID character = BattleGridUtils.GetCharacterID(ShowingAction.ActionOwner);
            List<Vector2Int> positions = new List<Vector2Int>();
            if (actionType == BattleActionType.Defend)
            {
                positions.Add(allyAction.position + BattleManager.Instance.CharacterManagement.Characters[character].CurrentPosition);
                if (allyAction.position != Vector2Int.zero)
                    positions.Add(BattleManager.Instance.CharacterManagement.Characters[character].CurrentPosition);
            }
            else if (actionType == BattleActionType.MoveFast || actionType == BattleActionType.MoveSafely)
            {
                positions.Add(allyAction.position);
            }
            else
            {
                BattleManager.Instance.CalculateActionRange(actionType, character);
                positions = BattleManager.Instance.BattleGrid.PositionsInRange;
            }
            BattleGridUI.Instance.ToggleRange(positions, actionType);
        }
        else if (ShowingAction as BossAction != null)
        {
            BossAction bossAction = ShowingAction as BossAction;
            List<Vector2Int> positions = bossAction.PreviewPositions;
            BattleGridUI.Instance.ToggleRange(positions, actionType);
        }
    }

    public void SetAsExecuting(int selectedAction, UIStatus status)
    {
        if (selectedAction == 0)
        {
            currentIndex = 0;
            topElement = 0;
            BattleUIManager.Instance.ActionPileUI.RefreshView(topElement, currentIndex);
            BattleUIManager.Instance.ActionPileUI.UpdateStatus(0, status);
        }
        else
        {
            topElement = selectedAction - 1;
            currentIndex = 1;
            BattleUIManager.Instance.ActionPileUI.RefreshView(topElement, currentIndex);
            BattleUIManager.Instance.ActionPileUI.UpdateStatus(0, UIStatus.Overdue);
            BattleUIManager.Instance.ActionPileUI.UpdateStatus(1, status);
        }

    }

    public void Refresh()
    {
        currentIndex = 0;
        topElement = 0;
        BattleUIManager.Instance.ActionPileUI.RefreshView(topElement, currentIndex);
        if (focus)
            PaintPreviewRange();
        else
            BattleGridUI.Instance.ToggleRange();
    }

    private ScheduledAction ShowingAction { get { return BattleManager.Instance.ActionPile.ActionsForTurn[topElement + currentIndex]; } }
    public long SelectedActionID { get { return BattleManager.Instance.ActionPile.ActionsForTurn[topElement + currentIndex].actionID; } }
    public bool CurrentlyIsCharacter { get { return BattleGridUtils.IsACharacter(BattleManager.Instance.ActionPile.ActionsForTurn[topElement + currentIndex].ActionOwner); } }
}