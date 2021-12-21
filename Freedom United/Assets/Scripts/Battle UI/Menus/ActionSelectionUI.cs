using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionUI : MonoBehaviour, NotificationsListener
{
    [SerializeField] private ActionSelectionOption[] actionPreviews;
    [SerializeField] private GameObject sectionContainer;

    public void ConfigureComponent()
    {
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleLoaded, Initialize);
    }

    private void Initialize(GameNotificationData notificationData)
    {
        for (int i = 0; i < actionPreviews.Length; i++)
        {
            actionPreviews[i].Config(BattleActionsUtils.GetActionsList()[i]);
        }
        ToggleVisible(false);
    }

    public void ToggleVisible(bool visible)
    {
        sectionContainer.SetActive(visible);
        if (visible)
            RefreshSelectedAction(0);
    }

    public void RefreshSelectedAction(int selectedCharacter)
    {
        for (int i = 0; i < actionPreviews.Length; i++)
        {
            actionPreviews[i].ToggleSelected(i == selectedCharacter);
        }
    }
}
