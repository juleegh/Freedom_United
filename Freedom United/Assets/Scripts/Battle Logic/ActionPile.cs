using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActionPile : MonoBehaviour, NotificationsListener
{

    private List<CharacterID> charactersAvailable;
    public List<CharacterID> CharactersAvailable { get { return charactersAvailable; } }

    private AllyAction currentAction { get { return BattleUINavigation.Instance.NavigationState.currentAction; } }
    private Dictionary<CharacterID, ScheduledAction> actionsForTurn;

    public void ConfigureComponent()
    {
        actionsForTurn = new Dictionary<CharacterID, ScheduledAction>();
        charactersAvailable = new List<CharacterID>();

        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleLoaded, Initialize);
    }

    private void Initialize(GameNotificationData notificationData)
    {
        foreach (CharacterID character in BattleManager.Instance.CharacterManagement.Characters.Keys.ToList())
            charactersAvailable.Add(character);
    }

    public void AddActionToPile(AllyAction action)
    {
        action.confirmed = true;
        actionsForTurn.Add(action.actionOwner, action);
        BattleUIManager.Instance.ActionPileUI.RefreshView(0, 0);
    }

    public void ClearList()
    {
        actionsForTurn.Clear();
        foreach (CharacterID character in BattleManager.Instance.CharacterManagement.Characters.Keys.ToList())
            charactersAvailable.Add(character);

        BattleUIManager.Instance.ActionPileUI.RefreshView(0, 0);
        BattleUIManager.Instance.CharacterSelectionUI.RefreshView(0, 0);
    }

    public List<ScheduledAction> ActionsForTurn
    {
        get
        {
            List<ScheduledAction> actionsToShow = actionsForTurn.Values.ToList();
            if (currentAction != null && !actionsForTurn.ContainsKey(currentAction.actionOwner) && currentAction.speed > 0)
            {
                actionsToShow.Add(currentAction);
                actionsToShow = actionsToShow.OrderBy(o => (o.speed)).ToList();
            }
            return actionsToShow;
        }
    }
}
