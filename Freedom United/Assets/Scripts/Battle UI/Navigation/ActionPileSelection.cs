public class ActionPileSelection : NavigationSelection
{
    protected override int MaxElements { get { return BattleManager.Instance.ActionPile.ActionsForTurn.Count; } }
    private int topElement;
    private int ElementsOnScreen { get { return BattleUIManager.Instance.ActionPileUI.ActionsOnScreen; } }

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
    }

    public void Refresh()
    {
        currentIndex = 0;
        topElement = 0;
        BattleUIManager.Instance.ActionPileUI.RefreshView(topElement, currentIndex);
    }
}