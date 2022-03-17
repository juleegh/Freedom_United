using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossRotateAction : ExecutingAction
{
    private BossPartType rotatingPart;
    private BossPart bossPart;
    private Vector2Int rotation;

    public BossRotateAction(BossAction scheduledAction) : base(scheduledAction)
    {
        rotatingPart = scheduledAction.actionOwner;
        bossPart = BattleManager.Instance.CharacterManagement.Boss.Parts[rotatingPart];
        rotation = scheduledAction.position;
    }

    public override void Execute()
    {
        if (rotatingPart == BattleManager.Instance.CharacterManagement.Boss.Core.PartType)
        {
            BattleManager.Instance.CharacterManagement.Boss.Rotate(rotation);
        }
        else
        {
            bossPart.Rotate(BattleManager.Instance.CharacterManagement.Boss.Core.GetCenterPosition(), rotation);
        }
        GameNotificationsManager.Instance.Notify(GameNotification.BossMoved);
    }
}