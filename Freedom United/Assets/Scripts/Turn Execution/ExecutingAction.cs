using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutingAction
{
    protected ScheduledAction representedAction;

    public ExecutingAction(ScheduledAction scheduledAction)
    {
        representedAction = scheduledAction;
    }

    public virtual void Execute()
    {
        Debug.LogError("Undefined action " + representedAction.ActionOwner + " - " + representedAction.actionType);
    }

    public bool CanPerform()
    {
        string actionOwner = representedAction.ActionOwner;
        if (BattleGridUtils.IsACharacter(actionOwner))
            return BattleManager.Instance.BattleValues.IsAlive(BattleGridUtils.GetCharacterID(actionOwner));
        else
            return BattleManager.Instance.BattleValues.BossPartIsDestroyed(BattleGridUtils.GetBossPart(actionOwner));
    }
}