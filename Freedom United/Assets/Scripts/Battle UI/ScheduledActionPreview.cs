using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScheduledActionPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionName;
    [SerializeField] private Image actorIcon;
    [SerializeField] private CharacterAssets characterAssets;
    [SerializeField] private BossesAssets bossesAssets;

    [SerializeField] private Image background;
    [SerializeField] private Color regularColor;
    [SerializeField] private Color selectedColor;

    public void ConfigVisuals(ScheduledAction scheduledAction)
    {
        actionName.text = scheduledAction.ActionType.ToString();
        if (BattleGridUtils.IsACharacter(scheduledAction.ActionOwner))
            actorIcon.sprite = characterAssets.Bodies[BattleGridUtils.GetCharacterID(scheduledAction.ActionOwner)];
        else
            actorIcon.sprite = bossesAssets.Parts[BattleGridUtils.GetBossPart(scheduledAction.ActionOwner)];
    }

    public void ToggleSelected(bool selected)
    {
        background.color = selected ? selectedColor : regularColor;
    }
}