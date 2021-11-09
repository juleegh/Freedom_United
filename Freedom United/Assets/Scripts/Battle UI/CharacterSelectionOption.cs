using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionOption : MonoBehaviour
{
    [SerializeField] private Image characterPreview;
    [SerializeField] private Image background;
    [SerializeField] private CharacterAssets assets;

    [SerializeField] private Color regularColor;
    [SerializeField] private Color selectedColor;

    CharacterID currentCharacter;

    public void Config(CharacterID character)
    {
        currentCharacter = character;
        characterPreview.sprite = assets.Bodies[character];
    }

    public void OnSelectionTriggered()
    {

    }

    public void ToggleSelected(bool selected)
    {
        background.color = selected ? selectedColor : regularColor;
    }
}
