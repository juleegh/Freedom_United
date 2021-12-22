using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleUINavigation : MonoBehaviour, NotificationsListener
{
    private static BattleUINavigation instance;
    public static BattleUINavigation Instance { get { return instance; } }

    private NavigationCurrentState navigationState;
    public NavigationCurrentState NavigationState { get { return navigationState; } }
    private NavigationActionExecuter actionExecuter;
    private NavigationSelectionChanger selectionChanger;

    public BattleSelectionLevel CurrentLevel { get { return navigationState.currentLevel; } }

    public void ConfigureComponent()
    {
        instance = this;
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.DependenciesLoaded, Initialize);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.TurnEndedExecution, ResetActionSelection);
    }

    private void Initialize(GameNotificationData notificationData)
    {
        navigationState = new NavigationCurrentState();
        actionExecuter = new NavigationActionExecuter();
        selectionChanger = new NavigationSelectionChanger();
        GameNotificationsManager.Instance.Notify(GameNotification.BattleUILoaded);
        GameNotificationsManager.Instance.Notify(GameNotification.TurnStarted);
        navigationState.ResetActionSelection();
    }

    private void ResetActionSelection(GameNotificationData notificationData)
    {
        navigationState.ResetActionSelection();
    }

    public void Down()
    {
        selectionChanger.Down();
    }

    public void Up()
    {
        selectionChanger.Up();
    }

    public void Left()
    {
        selectionChanger.Left();
    }

    public void Right()
    {
        selectionChanger.Right();
    }

    public void Forward()
    {
        actionExecuter.Forward();
    }

    public void Backwards()
    {
        actionExecuter.Backwards();
    }
}
