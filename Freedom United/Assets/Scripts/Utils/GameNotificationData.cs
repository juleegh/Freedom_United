using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNotificationData
{
    private Dictionary<string, object> data;
    public Dictionary<string, object> Data { get { return data; } }

    public GameNotificationData()
    {
        data = new Dictionary<string, object>();
    }
}