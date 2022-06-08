using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdditionalInfoScreen : MonoBehaviour, NotificationsListener
{
    [SerializeField] private GameObject actionInfoContainer;
    [SerializeField] private TextMeshProUGUI actionInfoTitle;
    [SerializeField] private TextMeshProUGUI actionInfoText;
    private Vector3 left;
    private Vector3 right;

    public void ConfigureComponent()
    {
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleUILoaded, Clear);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.NavigationInfoUpdated, NavigationInfoUpdated);
        right = actionInfoContainer.transform.position;
        left = right;
        left.x = 0;
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
                actionInfoContainer.transform.position = GetScreenPosition();
                actionInfoTitle.text = AdditionalInfoController.Instance.ActionTitle;
                actionInfoText.text = AdditionalInfoController.Instance.ActionDescription;
                break;
            case BattleSelectionLevel.Cell:
                actionInfoContainer.SetActive(BattleManager.Instance.BattleGrid.PositionsInRange.Contains(BattleUINavigation.Instance.NavigationState.currentAction.position));
                actionInfoContainer.transform.position = GetScreenPosition();
                actionInfoTitle.text = AdditionalInfoController.Instance.ActionTitle;
                actionInfoText.text = AdditionalInfoController.Instance.ActionDescription;
                break;
            default:
                actionInfoContainer.SetActive(false);
                break;
        }
    }

    private Vector3 GetScreenPosition()
    {
        string actionOwner = BattleUINavigation.Instance.NavigationState.currentAction.ActionOwner;

        if (BattleGridUtils.IsACharacter(actionOwner))
        { 
            CharacterID character = BattleGridUtils.GetCharacterID(actionOwner);
            Vector2Int currentPosition = BattleManager.Instance.CharacterManagement.Characters[character].CurrentPosition;
            int columns = BattleManager.Instance.BattleGrid.FurthestPosition.x;
            return currentPosition.x >= columns / 2 ? left : right;
        }
        return right;
    }
}
