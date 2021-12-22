using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyAction : ScheduledAction
{
    public CharacterID actionOwner;
    public override string ActionOwner { get { return actionOwner.ToString(); } }
    public Vector2Int position;
}