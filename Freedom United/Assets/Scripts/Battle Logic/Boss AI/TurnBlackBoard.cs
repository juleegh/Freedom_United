using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBlackBoard : MonoBehaviour, NotificationsListener
{
    private static TurnBlackBoard instance;
    public static TurnBlackBoard Instance { get { return instance; } }

    private PostActionInfo currentRegister;
    private List<PostActionInfo> registers;
    public List<PostActionInfo> Registers { get { return registers; } }

    public void ConfigureComponent()
    {
        instance = this;
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.AttackWasExecuted, RegisterAttackInfo);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.ActionEndedExecution, SaveRegister);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.TurnStartedExecution, StartRegisteringTurn);
        registers = new List<PostActionInfo>();
    }

    private void StartRegisteringTurn(GameNotificationData notificationData)
    {
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
}
