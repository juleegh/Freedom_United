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
}