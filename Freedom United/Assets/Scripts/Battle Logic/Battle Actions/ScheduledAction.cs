using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduledAction
{
    public BattleActionType actionType;
    public int speed;
    public Vector2Int position;
    public bool confirmed;

    public virtual string ActionOwner { get { return "Empty"; } }

    public ScheduledAction()
    {
        confirmed = false;
    }
}