using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterSelectionUI : MonoBehaviour, NotificationsListener
{
    [SerializeField] private int charactersOnScreen;
    [SerializeField] private Transform charactersContainer;
    [SerializeField] private GameObject characterPreviewPrefab;
    [SerializeField] private GameObject previousIndicator;
    [SerializeField] private GameObject nextIndicator;
    private CharacterSelectionOption[] characterPreviews;
    public int CharactersOnScreen { get { return charactersOnScreen; } }
    private bool focus { get { return BattleUINavigation.Instance.CurrentLevel == BattleSelectionLevel.Character; } }

    public void ConfigureComponent()
    {
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleLoaded, LoadUI);
    }

    private void LoadUI()
    {
        characterPreviews = new CharacterSelectionOption[charactersOnScreen];
        List<CharacterID> characters = BattleManager.Instance.CharacterManagement.Characters.Keys.ToList();

        for (int i = 0; i < charactersOnScreen; i++)
        {
            characterPreviews[i] = Instantiate(characterPreviewPrefab).GetComponent<CharacterSelectionOption>();
            characterPreviews[i].transform.SetParent(charactersContainer);
            characterPreviews[i].gameObject.SetActive(i < characters.Count);
        }
        RefreshView(0, 0);
    }

    public void RefreshView(int topCharacter, int selectedCharacter)
    {
        int previewIndex = 0;

        List<CharacterID> characters = BattleManager.Instance.CharacterManagement.Characters.Keys.ToList();
        for (int i = topCharacter; i < topCharacter + charactersOnScreen; i++)
        {
            if (i >= characters.Count)
            {
                previewIndex++;
                continue;
            }

            characterPreviews[previewIndex].Config(characters[i]);
            characterPreviews[previewIndex].AvailableForAction(BattleManager.Instance.ActionPile.CharactersAvailable.Contains(characters[i]));
            previewIndex++;
        }

        previousIndicator.SetActive(topCharacter > 0);
        nextIndicator.SetActive(topCharacter + charactersOnScreen <= characters.Count - 1);
        RefreshSelectedCharacter(selectedCharacter);
    }

    public void RefreshSelectedCharacter(int selectedCharacter)
    {
        for (int i = 0; i < charactersOnScreen; i++)
        {
            characterPreviews[i].ToggleSelected(i == selectedCharacter && focus);
        }
    }
}
