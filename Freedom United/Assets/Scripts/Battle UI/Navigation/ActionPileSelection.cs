public class ActionPileSelection : NavigationSelection
{
    protected override int MaxElements { get { return BattleManager.Instance.ActionPile.ActionsForTurn.Count; } }

    public override void Next()
    {
        if (currentIndex == MaxElements)
            return;

        currentIndex++;
        BattleUIManager.Instance.ActionPileUI.RefreshSelectedPreview(currentIndex);
    }
}