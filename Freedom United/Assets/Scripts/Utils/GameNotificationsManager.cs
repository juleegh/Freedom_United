using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameNotificationsManager : MonoBehaviour
{
    private static GameNotificationsManager instance;
    public static GameNotificationsManager Instance { get { return instance; } }
    private Dictionary<GameNotification, List<Action<GameNotificationData>>> linkedEvents;

    private IEnumerable<NotificationsListener> globalComponents;

    void Awake()
    {
        instance = this;
        linkedEvents = new Dictionary<GameNotification, List<Action<GameNotificationData>>>();

        globalComponents = FindObjectsOfType<MonoBehaviour>().OfType<NotificationsListener>();
        foreach (NotificationsListener comp in globalComponents)
        {
            comp.ConfigureComponent();
        }

        Notify(GameNotification.DependenciesLoaded);
    }

    public void AddActionToEvent(GameNotification typeEvent, Action<GameNotificationData> newAction)
    {
        if (!linkedEvents.ContainsKey(typeEvent))
            linkedEvents.Add(typeEvent, new List<Action<GameNotificationData>>());
        if (linkedEvents[typeEvent] == null)
            linkedEvents[typeEvent] = new List<Action<GameNotificationData>>();

        linkedEvents[typeEvent].Add(newAction);
    }

    public void Notify(GameNotification typeEvent, GameNotificationData additionalInfo = null)
    {
        if (!linkedEvents.ContainsKey(typeEvent) || linkedEvents[typeEvent] == null)
            return;

        foreach (Action<GameNotificationData> linkedAction in linkedEvents[typeEvent])
        {
            linkedAction(additionalInfo);
        }
    }
}
