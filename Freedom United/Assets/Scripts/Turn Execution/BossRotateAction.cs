using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossRotateAction : ExecutingAction
{
    private BossPartType rotatingPart;
    private Vector2Int rotation;

    public BossRotateAction(BossAction scheduledAction) : base(scheduledAction)
    {
        rotatingPart = scheduledAction.actionOwner;
        rotation = scheduledAction.position;
    }

    public override void Execute()
    {
        BattleManager.Instance.CharacterManagement.Boss.Rotate(rotation);
    }
}