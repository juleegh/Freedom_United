using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelUI : MonoBehaviour, NotificationsListener
{
    [SerializeField] private GameObject sectionContainer;

    public void ConfigureComponent()
    {
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleLoaded, Initialize);
    }

    private void Initialize(GameNotificationData notificationData)
    {
        ToggleVisible(false);
    }

    public void ToggleVisible(bool visible)
    {
        sectionContainer.SetActive(visible);
    }
}
