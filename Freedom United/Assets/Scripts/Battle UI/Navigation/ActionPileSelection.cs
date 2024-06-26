using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPileSelection : NavigationSelection
{
    protected override int MaxElement { get { return BattleManager.Instance.ActionPile.ActionsForTurn.Count - 1; } }
    private int topElement;
    private int ElementsOnScreen { get { return BattleUIManager.Instance.ActionPileUI.ActionsOnScreen; } }
    private bool focus { get { return BattleUINavigation.Instance.CurrentLevel == BattleSelectionLevel.ActionPile; } }

    public override void Next()
    {
        if (currentIndex + topElement == MaxElement)
            return;
        
        TogglePreviewOwner(false);
        if (currentIndex < ElementsOnScreen - 1 && currentIndex < MaxElement)
        {
            currentIndex++;
            BattleUIManager.Instance.ActionPileUI.RefreshView(topElement, currentIndex);
        }
        else if (topElement + ElementsOnScreen <= MaxElement)
        {
            topElement++;
            BattleUIManager.Instance.ActionPileUI.RefreshView(topElement, currentIndex);
        }
        PaintPreviewRange();
        TogglePreviewOwner(true);
    }

    public override void Previous()
    {
        if (currentIndex + topElement == 0)
            return;

        TogglePreviewOwner(false);
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
        TogglePreviewOwner(true);
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
                CharacterAttackAction attackAction = new CharacterAttackAction(allyAction);
                Vector2Int characterPos = BattleManager.Instance.CharacterManagement.Characters[character].CurrentPosition;
                foreach (Vector2Int pos in attackAction.GetAttackPositions())
                { 
                    positions.Add(characterPos + pos);
                }
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

    private void TogglePreviewOwner(bool visible)
    {
        BattleActionType actionType = ShowingAction.actionType;
        if (ShowingAction as AllyAction != null)
        {
            AllyAction allyAction = ShowingAction as AllyAction;
            CharacterID character = BattleGridUtils.GetCharacterID(ShowingAction.ActionOwner);
            Vector2Int currentPosition = BattleManager.Instance.CharacterManagement.Characters[character].CurrentPosition;
            BattleGridUI.Instance.ToggleHighlight(currentPosition, visible);
        }
        else if (ShowingAction as BossAction != null)
        {
            BossAction bossAction = ShowingAction as BossAction;
            BossPart bossPart = BattleManager.Instance.CharacterManagement.Boss.Parts[bossAction.actionOwner];
            List<Vector2Int> positions = bossPart.GetOccupiedPositions();
            BattleGridUI.Instance.ToggleHighlight(positions, visible);
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

    public void JumpToPreview()
    {
        int previewIndex = BattleManager.Instance.ActionPile.CurrentPreviewIndex;
        if (previewIndex == -1)
            return;

        if (previewIndex == 0)
        {
            currentIndex = 0;
            topElement = 0;
        }
        else
        {
            topElement = previewIndex - 1;
            currentIndex = 1;
        }
        BattleUIManager.Instance.ActionPileUI.RefreshView(topElement, currentIndex);
    }

    public void Refresh()
    {
        currentIndex = 0;
        topElement = 0;
        BattleUIManager.Instance.ActionPileUI.RefreshView(topElement, currentIndex);
        if (focus)
        { 
            TogglePreviewOwner(true);
            PaintPreviewRange();
        }
        else
        {
            TogglePreviewOwner(false);
            BattleGridUI.Instance.ToggleRange();
        }
    }

    private ScheduledAction ShowingAction { get { return BattleManager.Instance.ActionPile.ActionsForTurn[topElement + currentIndex]; } }
    public long SelectedActionID { get { return BattleManager.Instance.ActionPile.ActionsForTurn[topElement + currentIndex].actionID; } }
    public bool CurrentlyIsCharacter { get { return BattleGridUtils.IsACharacter(BattleManager.Instance.ActionPile.ActionsForTurn[topElement + currentIndex].ActionOwner); } }
}