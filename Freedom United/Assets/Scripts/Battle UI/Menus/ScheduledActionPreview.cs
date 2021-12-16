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
    [SerializeField] private Color currentColor;
    [SerializeField] private Color overdueColor;

    public void ConfigVisuals(ScheduledAction scheduledAction)
    {
        actionName.text = scheduledAction.actionType.ToString();
        if (BattleGridUtils.IsACharacter(scheduledAction.ActionOwner))
            actorIcon.sprite = characterAssets.Bodies[BattleGridUtils.GetCharacterID(scheduledAction.ActionOwner)];
        else
            actorIcon.sprite = bossesAssets.Parts[BattleGridUtils.GetBossPart(scheduledAction.ActionOwner)];
    }

    public void UpdateStatus(UIStatus status)
    {
        switch (status)
        {
            case UIStatus.Regular:
                background.color = regularColor;
                break;
            case UIStatus.Highlighted:
                background.color = selectedColor;
                break;
            case UIStatus.Current:
                background.color = currentColor;
                break;
            case UIStatus.Overdue:
                background.color = overdueColor;
                break;
        }
    }
}