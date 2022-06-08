using System.Linq;
using UnityEngine;

public class CharacterSelection : NavigationSelection
{
    protected override int MaxElement { get { return BattleManager.Instance.CharacterManagement.Characters.Keys.Count; } }
    private int topElement;
    private int ElementsOnScreen { get { return BattleUIManager.Instance.CharacterSelectionUI.CharactersOnScreen; } }

    public override void Next()
    {
        if (currentIndex + topElement == MaxElement)
            return;

        if (!FinishTurnSelected)
        {
            Vector2Int currentPosition = BattleManager.Instance.CharacterManagement.Characters[CharacterID].CurrentPosition;
            BattleGridUI.Instance.ToggleHighlight(currentPosition, false);
        }

        if (currentIndex < ElementsOnScreen - 1 && currentIndex < MaxElement)
        {
            currentIndex++;
            BattleUIManager.Instance.CharacterSelectionUI.RefreshView(topElement, currentIndex);
        }
        else if (topElement + ElementsOnScreen + 1 <= MaxElement)
        {
            topElement++;
            BattleUIManager.Instance.CharacterSelectionUI.RefreshView(topElement, currentIndex);
        }

        if (!FinishTurnSelected)
        {
            Vector2Int currentPosition = BattleManager.Instance.CharacterManagement.Characters[CharacterID].CurrentPosition;
            BattleGridUI.Instance.ToggleHighlight(currentPosition, true);
        }
    }

    public override void Previous()
    {
        if (currentIndex + topElement == 0)
            return;

        if (!FinishTurnSelected)
        {
            Vector2Int currentPosition = BattleManager.Instance.CharacterManagement.Characters[CharacterID].CurrentPosition;
            BattleGridUI.Instance.ToggleHighlight(currentPosition, false);
        }

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

        if (!FinishTurnSelected)
        {
            Vector2Int currentPosition = BattleManager.Instance.CharacterManagement.Characters[CharacterID].CurrentPosition;
            BattleGridUI.Instance.ToggleHighlight(currentPosition, true);
        }

    }

    public void Refresh()
    {
        BattleUIManager.Instance.CharacterSelectionUI.RefreshView(topElement, currentIndex);
        if (!FinishTurnSelected)
        {
            Vector2Int currentPosition = BattleManager.Instance.CharacterManagement.Characters[CharacterID].CurrentPosition;
            BattleGridUI.Instance.ToggleHighlight(currentPosition, BattleUINavigation.Instance.CurrentLevel == BattleSelectionLevel.Character);
        }
    }

    public Vector3 SelectedPosition {  get { return BattleUIManager.Instance.CharacterSelectionUI.GetPositionByIndex(currentIndex); } }
    public bool FinishTurnSelected { get { return currentIndex + topElement == MaxElement; } }
    public CharacterID CharacterID { get { return BattleManager.Instance.CharacterManagement.Characters.Keys.ToList()[currentIndex + topElement]; } }
}