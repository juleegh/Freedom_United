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

        defenseProvided /= scheduledAction.deltaOfAction.Positions.Count;
        areaOfEffect = scheduledAction.deltaOfAction.Positions;
        defendedPositions = new List<Vector2Int>();
    }

    public override void Execute()
    {
        defendedPositions = areaOfEffect;

        CameraFocus.Instance.FocusForDefense(defendedPositions);
        foreach (Vector2Int pos in defendedPositions)
        {
            GameNotificationData defenseData = new GameNotificationData();
            defenseData.Data[NotificationDataIDs.CellPosition] = pos;
            defenseData.Data[NotificationDataIDs.ShieldState] = true;
            GameNotificationsManager.Instance.Notify(GameNotification.DefenseWasUpdated, defenseData);
        }
    }

    public bool PositionIsDefended(Vector2Int position)
    {
        return defendedPositions.Contains(position);
    }
}