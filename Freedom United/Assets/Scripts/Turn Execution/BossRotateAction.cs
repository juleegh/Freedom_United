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
        BattleManager.Instance.CharacterManagement.Boss.Rotate(bossPart, rotation);
        GameNotificationsManager.Instance.Notify(GameNotification.BossMoved);
    }
}