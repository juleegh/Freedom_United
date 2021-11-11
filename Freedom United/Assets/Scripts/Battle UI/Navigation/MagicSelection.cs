public class MagicSelection : NavigationSelection
{
    protected override int MaxElements { get { return BattleManager.Instance.MagicManagement.Spells.Count; } }
    private int topElement;
    private int ElementsOnScreen { get { return BattleUIManager.Instance.MagicSelectionUI.SpellsOnScreen; } }

    public override void Next()
    {
        if (currentIndex < ElementsOnScreen - 1)
        {
            currentIndex++;
            BattleUIManager.Instance.MagicSelectionUI.RefreshView(topElement, currentIndex);
        }
        else if (topElement + ElementsOnScreen + 1 <= MaxElements)
        {
            topElement++;
            BattleUIManager.Instance.MagicSelectionUI.RefreshView(topElement, currentIndex);
        }
    }

    public override void Previous()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            BattleUIManager.Instance.MagicSelectionUI.RefreshView(topElement, currentIndex);
        }
        else if (topElement > 0)
        {
            topElement--;
            BattleUIManager.Instance.MagicSelectionUI.RefreshView(topElement, currentIndex);
        }
    }

    public void Toggle(bool visible)
    {
        BattleUIManager.Instance.MagicSelectionUI.Toggle(visible);
        if (visible)
        {
            currentIndex = 0;
            topElement = 0;
        }
    }
}