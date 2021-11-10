public class MagicSelection : NavigationSelection
{
    protected override int MaxElements { get { return BattleManager.Instance.MagicManagement.Spells.Count; } }

    public override void Next()
    {
        if (currentIndex == MaxElements)
            return;

        currentIndex++;
        BattleUIManager.Instance.MagicSelectionUI.RefreshSelectionView(currentIndex);
    }
}