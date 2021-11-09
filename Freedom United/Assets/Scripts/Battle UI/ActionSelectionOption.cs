using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionSelectionOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionID;
    [SerializeField] private Image background;

    [SerializeField] private Color regularColor;
    [SerializeField] private Color selectedColor;

    void Awake()
    {
        ToggleSelected(false);
    }

    public void Config(BattleActionType actionType)
    {
        actionID.text = actionType.ToString();
    }

    public void ToggleSelected(bool selected)
    {
        background.color = selected ? selectedColor : regularColor;
    }
}
