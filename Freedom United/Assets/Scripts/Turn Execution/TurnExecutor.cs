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
            if (executionDelta >= ExecutionValues.Primitives.ActionDuration)
            {
                if (executionIndex >= actionsQueued.Count)
                {
                    executing = false;
                    BattleManager.Instance.ActionPile.ClearList();
                    CameraFocus.Instance.ClearFocus();
                    actionsQueued.Clear();
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
                GameAudio.Instance.AudioToEvent(AudioEvent.ActionProgressedInPile);
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

        foreach (Vector2Int obstacle in BattleManager.Instance.BattleGrid.ObstaclePositions)
        {
            if (obstacle + BattleManager.Instance.CharacterManagement.Boss.Orientation == position)
            {
                defense += BattleManager.Instance.BattleGrid.GetObstacleHP(obstacle);
            }
        }
        return defense;
    }

    public List<Vector2Int> GetDefendingPositions(Vector2Int position)
    {
        List<Vector2Int> defenders = new List<Vector2Int>();
        foreach (ExecutingAction currentAction in actionsQueued)
        {
            if (currentAction as CharacterDefenseAction != null)
            {
                CharacterDefenseAction defenseAction = currentAction as CharacterDefenseAction;
                Character character = BattleManager.Instance.CharacterManagement.Characters[defenseAction.DefendingCharacter];
                if (defenseAction.PositionIsDefended(position) && !defenders.Contains(character.CurrentPosition))
                    defenders.Add(character.CurrentPosition);
            }
        }

        foreach (Vector2Int obstacle in BattleManager.Instance.BattleGrid.ObstaclePositions)
        {
            if (obstacle + BattleManager.Instance.CharacterManagement.Boss.Orientation == position)
            {
                defenders.Add(obstacle);
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

    public bool DefendedByObstacle(Vector2Int position)
    {
        foreach (Vector2Int obstacle in BattleManager.Instance.BattleGrid.ObstaclePositions)
        {
            if (obstacle + BattleManager.Instance.CharacterManagement.Boss.Orientation == position)
            {
                return true;
            }
        }

        return false;
    }
}
