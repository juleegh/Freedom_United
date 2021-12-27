using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossDefenseAction : ExecutingAction
{
    private BossPartType defendingBossPart;
    private List<Vector2Int> defendedPositions;
    private List<Vector2Int> areaOfEffect;

    private float defenseProvided;
    public float DefenseProvided { get { return defenseProvided; } }

    public BossDefenseAction(BossAction scheduledAction) : base(scheduledAction)
    {
        defendingBossPart = scheduledAction.actionOwner;
        defenseProvided = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[defendingBossPart].BaseDefense;

        defenseProvided *= scheduledAction.areaOfEffect.Positions.Count;
        areaOfEffect = scheduledAction.areaOfEffect.Positions;
        defendedPositions = new List<Vector2Int>();
    }

    public override void Execute()
    {
        defendedPositions = areaOfEffect;

        foreach (Vector2Int pos in defendedPositions)
        {
            GameNotificationData defenseData = new GameNotificationData();
            defenseData.Data[NotificationDataIDs.CellPosition] = pos;
            GameNotificationsManager.Instance.Notify(GameNotification.DefenseWasExecuted, defenseData);
        }
    }

    public bool PositionIsDefended(Vector2Int position)
    {
        return defendedPositions.Contains(position);
    }
}