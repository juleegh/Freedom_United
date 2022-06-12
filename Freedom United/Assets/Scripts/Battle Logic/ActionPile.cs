using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ActionPile : MonoBehaviour, NotificationsListener
{
    private AllyAction currentAction { get { return BattleUINavigation.Instance.NavigationState.currentAction; } }
    private Dictionary<string, List<ScheduledAction>> actionsForTurn;
    [SerializeField] private int bossActionLimit;
    [SerializeField] private int characterActionLimit;

    public void ConfigureComponent()
    {
        actionsForTurn = new Dictionary<string, List<ScheduledAction>>();
    }

    public void AddActionToPile(ScheduledAction action)
    {
        action.confirmed = true;
        action.actionID = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

        if (!actionsForTurn.ContainsKey(action.ActionOwner))
            actionsForTurn.Add(action.ActionOwner, new List<ScheduledAction>());

        actionsForTurn[action.ActionOwner].Add(action);
        BattleUIManager.Instance.ActionPileUI.RefreshView();
    }

    public void RemoveActionFromPile(long actionID)
    {
        foreach (List<ScheduledAction> characterActions in actionsForTurn.Values)
        {
            foreach (ScheduledAction characterAction in characterActions)
            {
                if (characterAction.actionID == actionID)
                {
                    actionsForTurn[characterAction.ActionOwner].Remove(characterAction);
                    BattleUIManager.Instance.ActionPileUI.RefreshView();
                    return;
                }
            }
        }
    }

    public bool CharacterAvailable(CharacterID character)
    {
        string actionOwner = character.ToString();
        return !actionsForTurn.ContainsKey(actionOwner) || actionsForTurn[actionOwner].Count < characterActionLimit;
    }

    public void ClearList()
    {
        actionsForTurn.Clear();

        BattleUIManager.Instance.ActionPileUI.RefreshView();
        BattleUIManager.Instance.CharacterSelectionUI.RefreshView(0, 0);
    }

    public List<ScheduledAction> ActionsForTurn
    {
        get
        {
            List<ScheduledAction> actionsToShow = new List<ScheduledAction>();

            foreach (List<ScheduledAction> characterActions in actionsForTurn.Values)
                actionsToShow.AddRange(characterActions);

            if (currentAction != null && !currentAction.confirmed && currentAction.speed > 0)
                actionsToShow.Add(currentAction);

            actionsToShow = actionsToShow.OrderBy(o => (-o.speed)).ToList();
            return actionsToShow;
        }
    }

    public int CurrentPreviewIndex { get { return ActionsForTurn.IndexOf(currentAction); } }

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
