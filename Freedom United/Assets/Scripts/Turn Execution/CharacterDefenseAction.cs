using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterDefenseAction : ExecutingAction
{
    private CharacterID defendingCharacter;
    private List<Vector2Int> defendedPositions;
    private Vector2Int defenseDelta;

    public float DefenseProvided { get { return GetCurrentDefense(); } }
    public CharacterID DefendingCharacter { get { return defendingCharacter; } }
    private Vector2Int CurrentCharacterPosition { get { return BattleManager.Instance.CharacterManagement.Characters[defendingCharacter].CurrentPosition; } }
    public List<Vector2Int> DefendedPositions { get { return defendedPositions; } }

    public CharacterDefenseAction(AllyAction scheduledAction) : base(scheduledAction)
    {
        defendingCharacter = scheduledAction.actionOwner;
        defenseDelta = scheduledAction.position;
        defendedPositions = new List<Vector2Int>();
    }

    public override void Execute()
    {
        defendedPositions.Add(defenseDelta);

        GameNotificationData defenseData = new GameNotificationData();
        defenseData.Data[NotificationDataIDs.CellPosition] = CurrentCharacterPosition + defenseDelta;
        defenseData.Data[NotificationDataIDs.ShieldState] = true;
        GameNotificationsManager.Instance.Notify(GameNotification.DefenseWasUpdated, defenseData);

        if (defenseDelta != Vector2Int.zero)
        {
            defendedPositions.Add(Vector2Int.zero);
            defenseData.Data[NotificationDataIDs.CellPosition] = CurrentCharacterPosition;
            GameNotificationsManager.Instance.Notify(GameNotification.DefenseWasUpdated, defenseData);
        }

    }

    public bool PositionIsDefended(Vector2Int position)
    {
        if (BattleManager.Instance.BattleValues.PartyDefense[defendingCharacter] <= 0)
            return false;

        return defendedPositions.Contains(position - CurrentCharacterPosition) && BattleManager.Instance.BattleValues.CanDefend(defendingCharacter);
    }

    private float GetCurrentDefense()
    {
        float defenseProvided = BattleManager.Instance.BattleValues.PartyDefense[defendingCharacter];

        if (defenseDelta != Vector2Int.zero)
            defenseProvided *= BattleGridUtils.DefenseSplitFactor;

        return defenseProvided;
    }
}