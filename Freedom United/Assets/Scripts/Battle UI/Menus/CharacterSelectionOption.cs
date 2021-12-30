using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionOption : MonoBehaviour
{
    [SerializeField] private Image characterPreview;
    [SerializeField] private Image background;
    [SerializeField] private CharacterAssets assets;
    [SerializeField] private GameObject blocked;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterHealth;
    [SerializeField] private TextMeshProUGUI characterWill;
    [SerializeField] private GameObject characterContent;
    [SerializeField] private GameObject otherContent;

    [SerializeField] private Color regularColor;
    [SerializeField] private Color selectedColor;

    CharacterID currentCharacter;

    public void Config(CharacterID character)
    {
        currentCharacter = character;
        characterPreview.sprite = assets.Bodies[character];
        UpdateInfo();
        blocked.SetActive(false);
        ToggleCharacterView(true);
    }

    public void AvailableForAction(bool available)
    {
        blocked.SetActive(!available);
    }

    private void UpdateInfo()
    {
        characterName.text = currentCharacter.ToString();
        characterHealth.text = "HP: " + BattleManager.Instance.BattleValues.PartyHealth[currentCharacter] + " / " + BattleManager.Instance.PartyStats.Stats[currentCharacter].BaseHealth;
        characterWill.text = "WP: " + BattleManager.Instance.BattleValues.PartyWill[currentCharacter] + " / " + BattleManager.Instance.PartyStats.Stats[currentCharacter].BaseWillPower;
    }

    public void OnSelectionTriggered()
    {

    }

    public void ToggleSelected(bool selected)
    {
        background.color = selected ? selectedColor : regularColor;
    }

    public void ToggleCharacterView(bool visible)
    {
        characterContent.SetActive(visible);
        otherContent.SetActive(!visible);
    }
}
