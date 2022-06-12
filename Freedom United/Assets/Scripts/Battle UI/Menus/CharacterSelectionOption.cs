using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionOption : MonoBehaviour
{
    [SerializeField] private CharacterSelectionInfo selectedOption;
    [SerializeField] private CharacterSelectionInfo regularOption;

    public void Config(CharacterID character)
    {
        selectedOption.Config(character);
        regularOption.Config(character);
    }

    public void AvailableForAction(bool available)
    {
        selectedOption.AvailableForAction(available);
        regularOption.AvailableForAction(available);
    }

    public void ToggleSelected(bool selected)
    {
        selectedOption.gameObject.SetActive(selected);
        regularOption.gameObject.SetActive(!selected);
    }

    public void ToggleCharacterView(bool visible)
    {
        selectedOption.ToggleCharacterView(visible);
        regularOption.ToggleCharacterView(visible);
    }
}
