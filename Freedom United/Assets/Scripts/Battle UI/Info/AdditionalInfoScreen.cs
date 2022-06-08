using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdditionalInfoScreen : MonoBehaviour, NotificationsListener
{
    [SerializeField] private GameObject actionInfoContainer;
    [SerializeField] private TextMeshProUGUI actionInfoTitle;
    [SerializeField] private TextMeshProUGUI actionInfoText;

    public void ConfigureComponent()
    {
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleUILoaded, Clear);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.NavigationInfoUpdated, NavigationInfoUpdated);
    }

    private void Clear(GameNotificationData notificationData)
    {
        actionInfoContainer.SetActive(false);
    }

    private void NavigationInfoUpdated(GameNotificationData notificationData)
    {
        switch (BattleUINavigation.Instance.CurrentLevel)
        {
            case BattleSelectionLevel.Action:
                actionInfoContainer.SetActive(true);
                actionInfoTitle.text = AdditionalInfoController.Instance.ActionTitle;
                actionInfoText.text = AdditionalInfoController.Instance.ActionDescription;
                break;
            case BattleSelectionLevel.Cell:
                actionInfoContainer.SetActive(BattleManager.Instance.BattleGrid.PositionsInRange.Contains(BattleUINavigation.Instance.NavigationState.currentAction.position));
                actionInfoTitle.text = AdditionalInfoController.Instance.ActionTitle;
                actionInfoText.text = AdditionalInfoController.Instance.ActionDescription;
                break;
            default:
                actionInfoContainer.SetActive(false);
                break;
        }
    }
}
