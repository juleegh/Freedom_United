using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdditionalInfoScreen : MonoBehaviour, NotificationsListener
{
    [SerializeField] private GameObject actionInfoContainer;
    [SerializeField] private GameObject actionTargetContainer;
    [SerializeField] private TextMeshProUGUI actionInfoTitle;
    [SerializeField] private TextMeshProUGUI actionInfoText;
    [SerializeField] private TextMeshProUGUI actionTargetText;

    public void ConfigureComponent()
    {
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleUILoaded, Clear);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.NavigationInfoUpdated, NavigationInfoUpdated);
    }

    private void Clear()
    {
        actionInfoContainer.SetActive(false);
        actionTargetContainer.SetActive(false);
    }

    private void NavigationInfoUpdated()
    {
        switch (BattleUINavigation.Instance.CurrentLevel)
        {
            case BattleSelectionLevel.Action:
                actionInfoContainer.SetActive(true);
                actionTargetContainer.SetActive(false);
                actionInfoTitle.text = AdditionalInfoController.Instance.ActionTitle;
                actionInfoText.text = AdditionalInfoController.Instance.ActionDescription;
                break;
            case BattleSelectionLevel.Cell:
                actionInfoContainer.SetActive(true);
                actionTargetContainer.SetActive(BattleManager.Instance.BattleGrid.PositionsInRange.Contains(BattleUINavigation.Instance.NavigationState.currentAction.position));
                actionInfoTitle.text = AdditionalInfoController.Instance.ActionTitle;
                actionInfoText.text = AdditionalInfoController.Instance.ActionDescription;
                actionTargetText.text = AdditionalInfoController.Instance.ActionTarget;
                break;
            default:
                actionInfoContainer.SetActive(false);
                actionTargetContainer.SetActive(false);
                break;
        }
    }
}
