using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : ScheduledAction
{
    public BossPartType actionOwner;
    public override string ActionOwner { get { return actionOwner.ToString(); } }
}