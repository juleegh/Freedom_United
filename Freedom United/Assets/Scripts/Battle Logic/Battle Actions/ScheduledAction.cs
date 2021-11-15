using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduledAction
{
    public BattleActionType actionType;
    public int speed;
    public Vector2Int position;

    public virtual string ActionOwner { get { return "Empty"; } }

    public ScheduledAction()
    {

    }
}