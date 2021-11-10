public class ActionSelection : NavigationSelection
{
    protected override int MaxElements { get { return 3; } }

    public override void Next()
    {
        if (currentIndex == MaxElements)
            return;

        currentIndex++;
        BattleUIManager.Instance.ActionSelectionUI.RefreshSelectedAction(currentIndex);
    }

    public override void Previous()
    {
        if (currentIndex == 0)
            return;
    }
}