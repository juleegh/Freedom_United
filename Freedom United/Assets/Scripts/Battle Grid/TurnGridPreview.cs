using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnGridPreview : MonoBehaviour, NotificationsListener
{
    private static TurnGridPreview instance;
    public static TurnGridPreview Instance { get { return instance; } }

    [SerializeField] private Transform cellsContainer;
    Dictionary<Vector2Int, TurnCellPreview> grid;

    public void ConfigureComponent()
    {
        instance = this;
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleGridLoaded, InitializeGrid);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.ActionPileModified, UpdatePreview);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.TurnStartedExecution, ClearPreview);
    }

    private void InitializeGrid(GameNotificationData notificationData)
    {
        grid = new Dictionary<Vector2Int, TurnCellPreview>();

        TurnCellPreview[] cells = cellsContainer.GetComponentsInChildren<TurnCellPreview>();
        foreach (TurnCellPreview cell in cells)
        {
            Vector3Int roundedPos = Vector3Int.RoundToInt(cell.transform.position);
            Vector2Int position = new Vector2Int(roundedPos.x, roundedPos.z);
            grid.Add(position, cell);
        }
    }

    private void UpdatePreview(GameNotificationData notificationData)
    {
        ClearPreview(null);

        List<Vector2Int> defensePositions = new List<Vector2Int>();

        PaintAttackActions();
        PaintDefenseActions();
        PaintMoveActions();
    }

    private void PaintAttackActions()
    {
        List<ScheduledAction> scheduledActions = BattleManager.Instance.ActionPile.ActionsForTurn;
        int actionQuantity = scheduledActions.Count;

        for (int i = 0; i < actionQuantity; i++)
        {
            ScheduledAction scheduledAction = scheduledActions[i];
            if (scheduledAction.actionType != BattleActionType.Attack)
                continue;

            if (BattleGridUtils.IsACharacter(scheduledAction.ActionOwner))
            {
                AllyAction allyAction = scheduledAction as AllyAction;
                CharacterAttackAction attackAction = new CharacterAttackAction(allyAction);
                Vector2Int positionByTurn = BattleManager.Instance.ActionPile.GetPositionByActionIndex(allyAction.actionOwner, i);
                foreach (Vector2Int attackPos in attackAction.GetAttackPositions())
                {
                    if (grid.ContainsKey(attackPos + positionByTurn))
                        grid[attackPos + positionByTurn].CreateAttackPreview();
                }
            }
            else
            {
                BossAction bossAction = scheduledAction as BossAction;
                BossAttackAction attackAction = new BossAttackAction(bossAction);

                foreach (Vector2Int attackPos in attackAction.GetTargetPositions())
                {
                    if (grid.ContainsKey(attackPos))
                        grid[attackPos].CreateAttackPreview();
                }
            }
        }
    }

    private void PaintDefenseActions()
    {
        List<ScheduledAction> scheduledActions = BattleManager.Instance.ActionPile.ActionsForTurn;
        int actionQuantity = scheduledActions.Count;

        for (int i = 0; i < actionQuantity; i++)
        {
            ScheduledAction scheduledAction = scheduledActions[i];
            if (scheduledAction.actionType != BattleActionType.Defend)
                continue;

            if (BattleGridUtils.IsACharacter(scheduledAction.ActionOwner))
            {
                AllyAction allyAction = scheduledAction as AllyAction;
                CharacterDefenseAction defenseAction = new CharacterDefenseAction(allyAction);
                Vector2Int positionByTurn = BattleManager.Instance.ActionPile.GetPositionByActionIndex(allyAction.actionOwner, i);
                foreach (Vector2Int defensePos in defenseAction.PotentiallyDefendedPositions(positionByTurn))
                {
                    if (grid.ContainsKey(defensePos))
                        grid[defensePos].CreateDefensePreview();
                }
            }
            else
            {
                BossAction bossAction = scheduledAction as BossAction;
                BossDefenseAction defenseAction = new BossDefenseAction(bossAction);

                foreach (Vector2Int defensePos in defenseAction.PotentialDefense)
                {
                    if (grid.ContainsKey(defensePos))
                        grid[defensePos].CreateDefensePreview();
                }
            }
        }
    }

    private void PaintMoveActions()
    {
        List<ScheduledAction> scheduledActions = BattleManager.Instance.ActionPile.ActionsForTurn;
        int actionQuantity = scheduledActions.Count;

        for (int i = 0; i < actionQuantity; i++)
        {
            ScheduledAction scheduledAction = scheduledActions[i];
            if (scheduledAction.actionType != BattleActionType.MoveFast && scheduledAction.actionType != BattleActionType.MoveSafely)
                continue;

            if (BattleGridUtils.IsACharacter(scheduledAction.ActionOwner))
            {
                AllyAction allyAction = scheduledAction as AllyAction;
                Character character = BattleManager.Instance.CharacterManagement.Characters[allyAction.actionOwner];
                Vector2Int positionByTurn = allyAction.position;
                Vector2Int current = character.CurrentPosition;
                
                if (current != positionByTurn)
                {
                    grid[positionByTurn].CreateCharacterPreview(character.CharacterID);
                }
            }
        }
    }

    private void ClearPreview(GameNotificationData notificationData)
    {
        foreach (KeyValuePair<Vector2Int, TurnCellPreview> cell in grid)
        {
            cell.Value.ClearPreview();
        }
    }
}
