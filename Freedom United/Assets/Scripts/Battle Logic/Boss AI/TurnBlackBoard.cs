using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBlackBoard : MonoBehaviour, NotificationsListener
{
    private PostActionInfo currentRegister;
    private List<PostActionInfo> registers;

    public void ConfigureComponent()
    {
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.AttackWasExecuted, RegisterAttackInfo);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.ActionEndedExecution, SaveRegister);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.TurnStartedExecution, StartRegisteringTurn);
    }

    private void StartRegisteringTurn(GameNotificationData notificationData)
    {
        if (registers == null)
            registers = new List<PostActionInfo>();

        registers.Clear();
        currentRegister = new PostActionInfo();
    }

    private void RegisterAttackInfo(GameNotificationData notificationData)
    {
        if (notificationData == null)
        {
            Debug.LogError("FATAL ERROR: Attack info empty");
            return;
        }

        currentRegister.actionOwner = (string)notificationData.Data[NotificationDataIDs.ActionOwner];
        currentRegister.actionTarget = (string)notificationData.Data[NotificationDataIDs.ActionTarget];
        currentRegister.wasFailure = (bool)notificationData.Data[NotificationDataIDs.Failure];
        currentRegister.actionType = BattleActionType.Attack;
        currentRegister.previousTargetHP = (float)notificationData.Data[NotificationDataIDs.PreviousHP];
        currentRegister.newTargetHP = (float)notificationData.Data[NotificationDataIDs.NewHP];
    }

    private void SaveRegister(GameNotificationData notificationData)
    {
        registers.Add(currentRegister);
        currentRegister = new PostActionInfo();
    }

    private TargetType GetTargetType(string targetId)
    {
        if (BattleGridUtils.IsACharacter(targetId))
            return TargetType.Character;
        else if (BattleGridUtils.IsABossPart(targetId))
            return TargetType.BossPart;

        return TargetType.Empty;
    }
}
