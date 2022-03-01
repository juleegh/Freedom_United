using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterDefenseAction : ExecutingAction
{
    private CharacterID defendingCharacter;
    private List<Vector2Int> defendedPositions;
    private Vector2Int defenseDelta;

    private float defenseProvided;
    public float DefenseProvided { get { return defenseProvided; } }
    public CharacterID DefendingCharacter { get { return defendingCharacter; } }
    private Vector2Int CurrentCharacterPosition { get { return BattleManager.Instance.CharacterManagement.Characters[defendingCharacter].CurrentPosition; } }

    public CharacterDefenseAction(AllyAction scheduledAction) : base(scheduledAction)
    {
        defendingCharacter = scheduledAction.actionOwner;
        defenseDelta = scheduledAction.position;
        defenseProvided = BattleManager.Instance.PartyStats.Stats[scheduledAction.actionOwner].BaseDefense;
        defendedPositions = new List<Vector2Int>();

        if (defenseDelta != Vector2Int.zero)
            defenseProvided *= BattleGridUtils.DefenseSplitFactor;
    }

    public override void Execute()
    {
        defendedPositions.Add(defenseDelta);

        GameNotificationData defenseData = new GameNotificationData();
        defenseData.Data[NotificationDataIDs.CellPosition] = CurrentCharacterPosition + defenseDelta;
        GameNotificationsManager.Instance.Notify(GameNotification.DefenseWasExecuted, defenseData);

        if (defenseDelta != Vector2Int.zero)
        {
            defendedPositions.Add(Vector2Int.zero);
            defenseData.Data[NotificationDataIDs.CellPosition] = CurrentCharacterPosition;
            GameNotificationsManager.Instance.Notify(GameNotification.DefenseWasExecuted, defenseData);
        }

    }

    public bool PositionIsDefended(Vector2Int position)
    {
        return defendedPositions.Contains(position - CurrentCharacterPosition);
    }
}