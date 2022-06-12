using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CharacterSelectionUI : MonoBehaviour, NotificationsListener
{
    [SerializeField] private int charactersOnScreen;
    [SerializeField] private Transform charactersContainer;
    [SerializeField] private GameObject characterPreviewPrefab;
    [SerializeField] private GameObject previousIndicator;
    [SerializeField] private GameObject nextIndicator;
    [SerializeField] private Transform selectedContainer;
    [SerializeField] private Transform regularContainer;
    [SerializeField] private LayoutGroup layoutGroup;

    private CharacterSelectionOption[] characterPreviews;
    public int CharactersOnScreen { get { return charactersOnScreen; } }
    private bool focus { get { return BattleUINavigation.Instance.CurrentLevel == BattleSelectionLevel.Character; } }

    public void ConfigureComponent()
    {
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleUILoaded, LoadUI);
    }

    private void LoadUI(GameNotificationData notificationData)
    {
        characterPreviews = new CharacterSelectionOption[charactersOnScreen];
        List<CharacterID> characters = BattleManager.Instance.CharacterManagement.Characters.Keys.ToList();

        for (int i = 0; i < charactersOnScreen; i++)
        {
            characterPreviews[i] = Instantiate(characterPreviewPrefab).GetComponent<CharacterSelectionOption>();
            characterPreviews[i].transform.SetParent(charactersContainer);
            characterPreviews[i].gameObject.SetActive(i < characters.Count + 1);
        }

        StartCoroutine(FinishConfig());
    }

    private IEnumerator FinishConfig()
    {
        yield return new WaitForEndOfFrame();
        layoutGroup.enabled = false;
        RefreshView(0, 0);
    }

    public void RefreshView(int topCharacter, int selectedCharacter)
    {
        int previewIndex = 0;

        List<CharacterID> characters = BattleManager.Instance.CharacterManagement.Characters.Keys.ToList();
        for (int i = topCharacter; i < topCharacter + charactersOnScreen; i++)
        {
            if (i == characters.Count)
            {
                characterPreviews[previewIndex].ToggleCharacterView(false);
                previewIndex++;
                continue;
            }
            else if (i > characters.Count)
            {
                previewIndex++;
                continue;
            }

            characterPreviews[previewIndex].ToggleCharacterView(true);
            characterPreviews[previewIndex].Config(characters[i]);
            characterPreviews[previewIndex].AvailableForAction(BattleManager.Instance.ActionPile.CharacterAvailable(characters[i]));
            previewIndex++;
        }

        previousIndicator.SetActive(topCharacter > 0);
        nextIndicator.SetActive(topCharacter + charactersOnScreen <= characters.Count);
        RefreshSelectedCharacter(selectedCharacter);
    }

    private void RefreshSelectedCharacter(int selectedCharacter)
    {
        for (int i = 0; i < charactersOnScreen; i++)
        {
            bool selected = i == selectedCharacter && focus;
            
            Vector3 pos = characterPreviews[i].transform.position;
            pos.z = selected ? 1f : 0;
            characterPreviews[i].transform.position = pos;
            
            characterPreviews[i].ToggleSelected(selected);
            characterPreviews[i].transform.SetParent(selected ? selectedContainer : regularContainer, true);
        }
    }

    public Vector3 GetPositionByIndex(int selectedCharacter)
    {
        if (selectedCharacter >= 0 && selectedCharacter < characterPreviews.Length)
        {
            return characterPreviews[selectedCharacter].transform.position;
        }

        return Vector3.zero;
    }
}
