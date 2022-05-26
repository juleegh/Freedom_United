using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnExecutor : MonoBehaviour, NotificationsListener
{
    private static TurnExecutor instance;
    public static TurnExecutor Instance { get { return instance; } }

    private List<ExecutingAction> actionsQueued;
    private int executionIndex;
    private bool executing;
    public bool Executing { get { return executing; } }

    [SerializeField] private float timeBetweenActions;
    private float executionDelta;
    private ActionPileSelection ActionPile { get { return BattleUINavigation.Instance.NavigationState.ActionPileSelection; } }

    public void ConfigureComponent()
    {
        instance = this;
        actionsQueued = new List<ExecutingAction>();
    }

    public void StartTurnExecution()
    {
        actionsQueued.Clear();
        foreach (ScheduledAction scheduledAction in BattleManager.Instance.ActionPile.ActionsForTurn)
        {
            actionsQueued.Add(ActionParser.GetParsedAction(scheduledAction));
        }
        GameNotificationsManager.Instance.Notify(GameNotification.TurnStartedExecution);
        executionIndex = 0;
        executionDelta = 0;
        executing = true;
    }

    private void Update()
    {
        if (executing)
        {
            executionDelta += Time.deltaTime;
            if (executionDelta >= timeBetweenActions)
            {
                if (executionIndex >= actionsQueued.Count)
                {
                    executing = false;
                    BattleManager.Instance.ActionPile.ClearList();
                    GameNotificationsManager.Instance.Notify(GameNotification.TurnEndedExecution);
                    GameNotificationsManager.Instance.Notify(GameNotification.TurnStarted);
                    return;
                }

                executionDelta = 0;
                bool canPerform = actionsQueued[executionIndex].CanPerform();
                if (canPerform)
                {
                    actionsQueued[executionIndex].Execute();
                }
                ActionPile.SetAsExecuting(executionIndex, canPerform ? UIStatus.Current : UIStatus.Invalid);
                executionIndex++;
                GameNotificationsManager.Instance.Notify(GameNotification.ActionEndedExecution);
            }
        }
    }

    public float DefenseValueInPosition(Vector2Int position)
    {
        float defense = 0f;
        foreach (ExecutingAction currentAction in actionsQueued)
        {
            if (currentAction as CharacterDefenseAction != null)
            {
                CharacterDefenseAction defenseAction = currentAction as CharacterDefenseAction;
                if (defenseAction.PositionIsDefended(position))
                    defense += defenseAction.DefenseProvided;
            }
            else if (currentAction as BossDefenseAction != null)
            {
                BossDefenseAction defenseAction = currentAction as BossDefenseAction;
                if (defenseAction.PositionIsDefended(position))
                    defense += defenseAction.DefenseProvided;
            }
        }
        return defense;
    }

    public List<CharacterID> GetDefendingCharacters(Vector2Int position)
    {
        List<CharacterID> defenders = new List<CharacterID>();
        foreach (ExecutingAction currentAction in actionsQueued)
        {
            if (currentAction as CharacterDefenseAction != null)
            {
                CharacterDefenseAction defenseAction = currentAction as CharacterDefenseAction;
                if (defenseAction.PositionIsDefended(position) && !defenders.Contains(defenseAction.DefendingCharacter))
                    defenders.Add(defenseAction.DefendingCharacter);
            }
        }
        return defenders;
    }

    public List<Vector2Int> GetDefendedPositions(CharacterID character)
    {
        List<Vector2Int> defendedPositions = new List<Vector2Int>();
        foreach (ExecutingAction currentAction in actionsQueued)
        {
            if (currentAction as CharacterDefenseAction != null)
            {
                CharacterDefenseAction defenseAction = currentAction as CharacterDefenseAction;
                if (defenseAction.DefendingCharacter == character)
                {
                    defendedPositions.AddRange(defenseAction.DefendedPositions);
                }
            }
        }
        return defendedPositions;
    }
}
