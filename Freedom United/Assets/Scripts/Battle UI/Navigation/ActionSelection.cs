public class ActionSelection : NavigationSelection
{
    protected override int MaxElements { get { return BattleActionsUtils.GetActionsList().Count; } }

    public override void Next()
    {
        if (currentIndex == MaxElements - 1)
            return;

        currentIndex++;
        BattleUIManager.Instance.ActionSelectionUI.RefreshSelectedAction(currentIndex);
    }

    public override void Previous()
    {
        if (currentIndex == 0)
            return;

        currentIndex--;
        BattleUIManager.Instance.ActionSelectionUI.RefreshSelectedAction(currentIndex);
    }

    public void Toggle(bool visible)
    {
        BattleUIManager.Instance.ActionSelectionUI.ToggleVisible(visible);
        if (visible)
        {
            currentIndex = 0;
            BattleUIManager.Instance.ActionSelectionUI.RefreshSelectedAction(currentIndex);
        }
    }

    public BattleActionType ActionSelected { get { return BattleActionsUtils.GetByIndex(currentIndex); } }
}