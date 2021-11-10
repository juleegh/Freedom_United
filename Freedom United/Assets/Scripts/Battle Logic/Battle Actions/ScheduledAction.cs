using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduledAction
{
    protected BattleActionType actionType;
    protected int speed;
    protected Vector2Int position;

    public int Speed { get { return speed; } }
    public virtual string ActionOwner { get { return "Empty"; } }
    public BattleActionType ActionType { get { return actionType; } }
    public Vector2Int Position { get { return position; } }

    public ScheduledAction(BattleActionType battleAction, Vector2Int pos, int actionSpeed)
    {
        actionType = battleAction;
        position = pos;
        speed = actionSpeed;
    }
}