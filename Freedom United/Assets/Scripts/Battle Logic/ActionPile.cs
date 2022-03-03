using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActionPile : MonoBehaviour, NotificationsListener
{
    private AllyAction currentAction { get { return BattleUINavigation.Instance.NavigationState.currentAction; } }
    private Dictionary<string, List<ScheduledAction>> actionsForTurn;
    [SerializeField] private int bossActionLimit;

    public void ConfigureComponent()
    {
        actionsForTurn = new Dictionary<string, List<ScheduledAction>>();
    }

    public void AddActionToPile(ScheduledAction action)
    {
        action.confirmed = true;

        if (!actionsForTurn.ContainsKey(action.ActionOwner))
            actionsForTurn.Add(action.ActionOwner, new List<ScheduledAction>());

        actionsForTurn[action.ActionOwner].Add(action);
        BattleUIManager.Instance.ActionPileUI.RefreshView(0, 0);
    }

    public void RemoveActionFromPile(CharacterID selectedCharacter)
    {
        actionsForTurn.Remove(selectedCharacter.ToString());
        BattleUIManager.Instance.ActionPileUI.RefreshView(0, 0);
    }

    public bool CharacterAvailable(CharacterID character)
    {
        string actionOwner = character.ToString();
        return !actionsForTurn.ContainsKey(actionOwner);
    }

    public void ClearList()
    {
        actionsForTurn.Clear();

        BattleUIManager.Instance.ActionPileUI.RefreshView(0, 0);
        BattleUIManager.Instance.CharacterSelectionUI.RefreshView(0, 0);
    }

    public List<ScheduledAction> ActionsForTurn
    {
        get
        {
            List<ScheduledAction> actionsToShow = new List<ScheduledAction>();

            foreach (List<ScheduledAction> characterActions in actionsForTurn.Values)
                actionsToShow.AddRange(characterActions);

            if (currentAction != null && !actionsForTurn.ContainsKey(currentAction.actionOwner.ToString()) && currentAction.speed > 0)
                actionsToShow.Add(currentAction);

            actionsToShow = actionsToShow.OrderBy(o => (-o.speed)).ToList();
            return actionsToShow;
        }
    }

    public bool BossReachedLimit
    {
        get
        {
            int bossActions = 0;
            foreach (KeyValuePair<string, List<ScheduledAction>> scheduledActions in actionsForTurn)
            {
                if (BattleGridUtils.IsABossPart(scheduledActions.Key))
                {
                    bossActions += scheduledActions.Value.Count;
                }
            }

            return bossActions >= bossActionLimit;
        }
    }

}
