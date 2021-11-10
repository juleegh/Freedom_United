using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyAction : ScheduledAction
{
    private CharacterID actionOwner;
    public override string ActionOwner { get { return actionOwner.ToString(); } }

    public AllyAction(CharacterID actor, BattleActionType battleAction, Vector2Int pos, int actionSpeed) : base(battleAction, pos, actionSpeed)
    {
        actionType = battleAction;
        position = pos;
        actionOwner = actor;
        speed = actionSpeed;
    }
}