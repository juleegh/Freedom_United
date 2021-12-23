using System.Linq;
using UnityEngine;

public class CharacterSelection : NavigationSelection
{
    protected override int MaxElements { get { return BattleManager.Instance.CharacterManagement.Characters.Keys.Count; } }
    private int topElement;
    private int ElementsOnScreen { get { return BattleUIManager.Instance.CharacterSelectionUI.CharactersOnScreen; } }

    public override void Next()
    {
        if (currentIndex + topElement == MaxElements - 1)
            return;

        if (currentIndex < ElementsOnScreen - 1 && currentIndex < MaxElements - 1)
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
        if (currentIndex + topElement == 0)
            return;

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

    public void Refresh()
    {
        currentIndex = 0;
        topElement = 0;

        while (currentIndex + topElement < MaxElements - 1 && !BattleManager.Instance.ActionPile.CharacterAvailable(CharacterID))
            Next();
        BattleUIManager.Instance.CharacterSelectionUI.RefreshView(topElement, currentIndex);
    }

    public Character SelectedCharacter { get { return BattleManager.Instance.CharacterManagement.Characters.Values.ToList()[currentIndex + topElement]; } }
    public CharacterID CharacterID { get { return BattleManager.Instance.CharacterManagement.Characters.Keys.ToList()[currentIndex + topElement]; } }
}