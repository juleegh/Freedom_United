using UnityEngine;

public class ActionSelection : NavigationSelection
{
    protected override int MaxElement { get { return BattleActionsUtils.GetActionsList().Count - 1; } }

    public override void Next()
    {
        if (currentIndex == MaxElement)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }

        BattleUIManager.Instance.ActionSelectionUI.RefreshSelectedAction(PreviousAction, ActionSelected, NextAction);
    }

    public override void Previous()
    {
        if (currentIndex == 0)
        {
            currentIndex = MaxElement;
        }
        else
        {
            currentIndex--;
        }

        BattleUIManager.Instance.ActionSelectionUI.RefreshSelectedAction(PreviousAction, ActionSelected, NextAction);
    }

    public void Toggle(bool visible)
    {
        BattleUIManager.Instance.ActionSelectionUI.ToggleVisible(visible);
        if (visible)
        {
            currentIndex = 0;
            Vector3 position = BattleUINavigation.Instance.NavigationState.CharacterSelection.SelectedPosition;
            BattleUIManager.Instance.ActionSelectionUI.SetHorizontalPosition(position);
            BattleUIManager.Instance.ActionSelectionUI.RefreshSelectedAction(PreviousAction, ActionSelected, NextAction);
        }
    }

    public BattleActionType PreviousAction { get { return currentIndex > 0 ? BattleActionsUtils.GetByIndex(currentIndex - 1) : BattleActionsUtils.GetByIndex(MaxElement); } }
    public BattleActionType NextAction { get { return currentIndex < MaxElement ? BattleActionsUtils.GetByIndex(currentIndex + 1) : BattleActionsUtils.GetByIndex(0); } }
    public BattleActionType ActionSelected { get { return BattleActionsUtils.GetByIndex(currentIndex); } }
}