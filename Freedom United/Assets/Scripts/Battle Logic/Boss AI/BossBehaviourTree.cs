using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossBehaviourTree : MonoBehaviour, NotificationsListener
{
    [SerializeField] private List<AITurnOption> turnOptions;

    public void ConfigureComponent()
    {
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.TurnStarted, ChooseActionsForTurn);
    }

    private void ChooseActionsForTurn(GameNotificationData notificationData)
    {
        foreach (AITurnOption turnOption in turnOptions)
        {
            turnOption.Evaluate();
            if (BattleManager.Instance.ActionPile.BossReachedLimit)
            {
                return;
            }
        }
    }
}
