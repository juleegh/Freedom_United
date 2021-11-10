using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : ScheduledAction
{
    private BossPartType actionOwner;
    public override string ActionOwner { get { return actionOwner.ToString(); } }

    public BossAction(BossPartType actor, BattleActionType battleAction, Vector2Int pos, int actionSpeed) : base(battleAction, pos, actionSpeed)
    {
        actionType = battleAction;
        position = pos;
        actionOwner = actor;
        speed = actionSpeed;
    }
}