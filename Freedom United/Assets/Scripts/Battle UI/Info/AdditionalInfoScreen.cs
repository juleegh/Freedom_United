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
        actionInfoTitle.text = "";
        actionInfoText.text = "";
    }

    private void NavigationInfoUpdated(GameNotificationData notificationData)
    {
        switch (BattleUINavigation.Instance.CurrentLevel)
        {
            case BattleSelectionLevel.Action:
                actionInfoTitle.text = AdditionalInfoController.Instance.ActionTitle;
                actionInfoText.text = AdditionalInfoController.Instance.ActionDescription;
                break;
            case BattleSelectionLevel.Cell:
                bool infoToPreview = BattleManager.Instance.BattleGrid.PositionsInRange.Contains(BattleUINavigation.Instance.NavigationState.currentAction.position);
                if (infoToPreview)
                {
                    actionInfoTitle.text = AdditionalInfoController.Instance.ActionTitle;
                    actionInfoText.text = AdditionalInfoController.Instance.ActionDescription;
                }
                else
                {
                    actionInfoTitle.text = "";
                    actionInfoText.text = "";
                }
                break;
            default:
                actionInfoTitle.text = "";
                actionInfoText.text = "";
                break;
        }
    }
}
