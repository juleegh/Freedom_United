public class CharacterSelection : NavigationSelection
{
    protected override int MaxElements { get { return BattleManager.Instance.CharacterManagement.Characters.Keys.Count; } }

    public override void Next()
    {
        if (currentIndex == MaxElements)
            return;

        currentIndex++;
        BattleUIManager.Instance.CharacterSelectionUI.RefreshSelectedCharacter(currentIndex);
    }
}