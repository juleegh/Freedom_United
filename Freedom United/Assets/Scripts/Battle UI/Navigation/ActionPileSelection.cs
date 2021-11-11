public class ActionPileSelection : NavigationSelection
{
    protected override int MaxElements { get { return BattleManager.Instance.ActionPile.ActionsForTurn.Count; } }
    private int topElement;
    private int ElementsOnScreen { get { return BattleUIManager.Instance.ActionPileUI.ActionsOnScreen; } }

    public override void Next()
    {
        if (currentIndex < ElementsOnScreen - 1)
        {
            currentIndex++;
            BattleUIManager.Instance.CharacterSelectionUI.RefreshView(topElement, currentIndex);
        }
        else if (topElement + ElementsOnScreen + 1 <= MaxElements)
        {
            topElement++;
            BattleUIManager.Instance.CharacterSelectionUI.RefreshView(topElement, currentIndex);
        }
    }

    public override void Previous()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            BattleUIManager.Instance.CharacterSelectionUI.RefreshView(topElement, currentIndex);
        }
        else if (topElement > 0)
        {
            topElement--;
            BattleUIManager.Instance.CharacterSelectionUI.RefreshView(topElement, currentIndex);
        }
    }
}