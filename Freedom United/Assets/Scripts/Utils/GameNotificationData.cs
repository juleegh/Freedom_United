using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNotificationData
{
    private Dictionary<NotificationDataIDs, object> data;
    public Dictionary<NotificationDataIDs, object> Data { get { return data; } }

    public GameNotificationData()
    {
        data = new Dictionary<NotificationDataIDs, object>();
    }
}