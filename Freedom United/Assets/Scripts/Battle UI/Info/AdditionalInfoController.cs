using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalInfoController : MonoBehaviour, NotificationsListener
{
    private static AdditionalInfoController instance;
    public static AdditionalInfoController Instance { get { return instance; } }

    private string actionTitle;
    private string actionOwner;
    private string actionDescription;
    private string actionTarget;

    public string ActionTitle { get { return actionTitle; } }
    public string ActionOwner { get { return actionOwner; } }
    public string ActionDescription { get { return actionDescription; } }
    public string ActionTarget { get { return actionTarget; } }

    private ActionInformationParser infoParser;

    public void ConfigureComponent()
    {
        instance = this;
        infoParser = new ActionInformationParser();
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.NavigationStateUpdated, NavigationStateUpdated);
    }

    private void NavigationStateUpdated()
    {
        switch (BattleUINavigation.Instance.CurrentLevel)
        {
            case BattleSelectionLevel.Action:
                actionTitle = infoParser.DefineActionTitle();
                actionOwner = BattleUINavigation.Instance.NavigationState.currentAction.ActionOwner.ToString();
                actionDescription = infoParser.DefineActionDescription();
                GameNotificationsManager.Instance.Notify(GameNotification.NavigationInfoUpdated);
                break;
            case BattleSelectionLevel.Cell:
                actionTitle = infoParser.DefineActionTitle();
                actionDescription = infoParser.DefineActionDescription();
                actionTarget = infoParser.DefineActionTarget();
                GameNotificationsManager.Instance.Notify(GameNotification.NavigationInfoUpdated);
                break;
            case BattleSelectionLevel.Character:
                GameNotificationsManager.Instance.Notify(GameNotification.NavigationInfoUpdated);
                break;
        }
    }
}
