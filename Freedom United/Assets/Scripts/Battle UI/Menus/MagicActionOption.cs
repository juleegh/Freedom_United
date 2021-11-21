using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MagicActionOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionID;
    [SerializeField] private Image background;

    [SerializeField] private Color regularColor;
    [SerializeField] private Color selectedColor;

    void Awake()
    {
        ToggleSelected(false);
    }

    public void Config(string spellName)
    {
        actionID.text = spellName;
    }

    public void ToggleSelected(bool selected)
    {
        background.color = selected ? selectedColor : regularColor;
    }
}
