using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBlackBoard : MonoBehaviour, NotificationsListener
{
    private static TurnBlackBoard instance;
    public static TurnBlackBoard Instance { get { return instance; } }

    private Dictionary<CharacterID, PositionAwareness> characterAwareness;
    private PostActionInfo currentRegister;
    private List<PostActionInfo> lastTurnRegisters;
    public List<PostActionInfo> LastTurnRegisters { get { return lastTurnRegisters; } }

    public void ConfigureComponent()
    {
        instance = this;
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.AttackWasExecuted, RegisterAttackInfo);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.CharacterMoved, RegisterMoveInfo);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.DefenseWasHit, RegisterDefenseInfo);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.TurnStartedExecution, StartRegisteringTurn);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.TurnEndedExecution, CleanCharacterAwareness);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleLoaded, CleanCharacterAwareness);
        lastTurnRegisters = new List<PostActionInfo>();
        characterAwareness = new Dictionary<CharacterID, PositionAwareness>();
    }

    private void StartRegisteringTurn(GameNotificationData notificationData)
    {
        lastTurnRegisters.Clear();
        currentRegister = new PostActionInfo();
    }

    private void CleanCharacterAwareness(GameNotificationData notificationData)
    {
        foreach (KeyValuePair<CharacterID, PositionAwareness> position in characterAwareness)
        {
            position.Value.turnsInMemory--;
            //Debug.LogError(position.Key + " -> " + position.Value.position + ": " + position.Value.turnsInMemory);
        }

        foreach (Character character in BattleManager.Instance.CharacterManagement.Characters.Values)
        {
            Vector2Int position = character.CurrentPosition;
            if (BattleManager.Instance.CharacterManagement.Boss.GetFieldOfView().Contains(position))
            {
                if (!BattleManager.Instance.BattleGrid.HidingPositions.Contains(position))
                {
                    RegisterCharacterAwareness(character.CharacterID, position);
                }
            }
        }
    }

    private void RegisterAttackInfo(GameNotificationData notificationData)
    {
        if (notificationData == null)
        {
            Debug.LogError("FATAL ERROR: Attack info empty");
            return;
        }

        string actionTarget = (string)notificationData.Data[NotificationDataIDs.ActionTarget];
        if (actionTarget == "empty")
            return;

        currentRegister.actionOwner = (string)notificationData.Data[NotificationDataIDs.ActionOwner];
        currentRegister.actionTarget = (string)notificationData.Data[NotificationDataIDs.ActionTarget];
        currentRegister.wasFailure = (bool)notificationData.Data[NotificationDataIDs.Failure];
        currentRegister.wasCritical = (bool)notificationData.Data[NotificationDataIDs.Critical];
        currentRegister.actionType = BattleActionType.Attack;
        currentRegister.previousTargetHP = (float)notificationData.Data[NotificationDataIDs.PreviousHP];
        currentRegister.newTargetHP = (float)notificationData.Data[NotificationDataIDs.NewHP];

        if (BattleActionsUtils.GetTargetType(currentRegister.actionOwner) == TargetType.Character && BattleActionsUtils.GetTargetType(currentRegister.actionTarget) == TargetType.BossPart)
        {
            CharacterID character = BattleGridUtils.GetCharacterID(currentRegister.actionOwner);
            RegisterCharacterAwareness(character, BattleManager.Instance.CharacterManagement.Characters[character].CurrentPosition);
        }

        lastTurnRegisters.Add(currentRegister);
        currentRegister = new PostActionInfo();
    }

    private void RegisterMoveInfo(GameNotificationData notificationData)
    {
        CharacterID character = (CharacterID)notificationData.Data[NotificationDataIDs.ActionOwner];
        Vector2Int newPosition = (Vector2Int)notificationData.Data[NotificationDataIDs.CellPosition];
        bool reckless = (bool)notificationData.Data[NotificationDataIDs.WasReckless];

        if (reckless)
            RegisterCharacterAwareness(character, newPosition);
    }

    private void RegisterDefenseInfo(GameNotificationData notificationData)
    {
        CharacterID character = (CharacterID)notificationData.Data[NotificationDataIDs.ActionOwner];
        Vector2Int newPosition = (Vector2Int)notificationData.Data[NotificationDataIDs.CellPosition];
        RegisterCharacterAwareness(character, newPosition);
    }

    private void RegisterCharacterAwareness(CharacterID character, Vector2Int position)
    {
        if (!characterAwareness.ContainsKey(character))
        {
            characterAwareness[character] = new PositionAwareness(position);
        }
        else
        {
            characterAwareness[character].Reset(position);
        }
    }

    public bool IsAwareOfCharacter(CharacterID character)
    {
        return characterAwareness.ContainsKey(character) && characterAwareness[character].turnsInMemory > 0;
    }

    public Vector2Int GetPositionForCharacter(CharacterID character)
    {
        if (!characterAwareness.ContainsKey(character))
            return -Vector2Int.one;

        return characterAwareness[character].position;
    }
}
