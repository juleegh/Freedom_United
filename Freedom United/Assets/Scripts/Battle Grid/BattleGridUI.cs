using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGridUI : MonoBehaviour, NotificationsListener
{
    private static BattleGridUI instance;
    public static BattleGridUI Instance { get { return instance; } }

    [SerializeField] private float cellDistance;
    [SerializeField] private Transform cellsContainer;
    [SerializeField] private Transform charactersContainer;
    [SerializeField] private BossVisuals bossVisuals;

    Dictionary<Vector2Int, GridCellUI> grid;
    Dictionary<CharacterID, CharacterVisuals> characters;
    private float charactersHeightOnBoard = 0.02f;


    public void ConfigureComponent()
    {
        instance = this;
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleLoaded, InitializeGrid);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.CharacterMoved, RefreshCharacters);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BossMoved, RefreshBoss);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.RecentDeath, RefreshDeaths);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.AttackWasExecuted, ShowAttackBattleAction);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.DefenseWasUpdated, ShowDefenseBattleAction);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.TurnEndedExecution, ClearBoardEffects);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.FieldOfViewChanged, UpdateBossFieldOfView);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.ObstaclesStatsChanged, RefreshObstacles);

    }

    private void InitializeGrid(GameNotificationData notificationData)
    {
        grid = new Dictionary<Vector2Int, GridCellUI>();

        GridCellUI[] cells = cellsContainer.GetComponentsInChildren<GridCellUI>();
        foreach (GridCellUI cell in cells)
        {
            Vector3Int roundedPos = Vector3Int.RoundToInt(cell.transform.position);
            Vector2Int position = new Vector2Int(roundedPos.x, roundedPos.z);
            grid.Add(position, cell);
        }

        characters = new Dictionary<CharacterID, CharacterVisuals>();
        CharacterVisuals[] charactersInGrid = charactersContainer.GetComponentsInChildren<CharacterVisuals>();

        foreach (CharacterVisuals characterVisuals in charactersInGrid)
        {
            characters.Add(characterVisuals.CharacterID, characterVisuals);
            characterVisuals.Initialize();
        }

        UpdateBossFieldOfView(null);
        RefreshBoss(null);
    }

    private void RefreshCharacters(GameNotificationData notificationData)
    {
        CharacterVisuals characterVisuals = characters[(CharacterID)notificationData.Data[NotificationDataIDs.ActionOwner]];
        Vector3 currentPosition = characterVisuals.transform.position;
        Vector2Int newPosition = (Vector2Int)notificationData.Data[NotificationDataIDs.CellPosition];
        Vector3 nextPosition = BattleGridUtils.TranslatedPosition(newPosition, charactersHeightOnBoard);

        grid[BattleGridUtils.GridPosition(currentPosition)].PromptMove();
        characterVisuals.transform.position = nextPosition;
        if ((bool)notificationData.Data[NotificationDataIDs.WasPushed])
        {
            grid[newPosition].PromptDamage(15);
        }
        UpdateDefenseInBoard();
    }

    private void RefreshBoss(GameNotificationData notificationData)
    {
        bossVisuals.Refresh(BattleManager.Instance.CharacterManagement.Boss);
    }

    private void RefreshObstacles(GameNotificationData notificationData)
    {
        Vector2Int position = (Vector2Int)notificationData.Data[NotificationDataIDs.CellPosition];
        GridCellUI newGridCell = grid[position];
        newGridCell.Refresh(BattleManager.Instance.BattleGrid.GetInPosition(position.x, position.y));
        BossPart partInPosition = BattleManager.Instance.CharacterManagement.GetBossPartInPosition(position);
        if (partInPosition != null)
            bossVisuals.RemovePart(partInPosition.PartType);
    }

    private void RefreshDeaths(GameNotificationData notificationData)
    {
        string dead = (string)notificationData.Data[NotificationDataIDs.Deceased];
        if (BattleGridUtils.IsACharacter(dead))
        {
            CharacterID character = BattleGridUtils.GetCharacterID(dead);
            CharacterVisuals characterVisuals = characters[character];
            characterVisuals.PaintDeath();
        }
        else
        {
            BossPartType bossPart = BattleGridUtils.GetBossPart(dead);
            bossVisuals.PaintDeath(bossPart);
        }
    }

    public void ToggleRange(List<Vector2Int> positions = null, BattleActionType actionType = BattleActionType.Attack)
    {
        foreach (KeyValuePair<Vector2Int, GridCellUI> cell in grid)
        {
            if (positions == null || !positions.Contains(cell.Key))
            {
                cell.Value.CleanRange();
                continue;
            }
            else
                cell.Value.PaintAsRange(actionType);
        }
    }

    private void ShowAttackBattleAction(GameNotificationData notificationData)
    {
        if (notificationData != null)
        {
            Vector2Int position = (Vector2Int)notificationData.Data[NotificationDataIDs.CellPosition];
            if (!grid.ContainsKey(position))
                return;
            bool failure = (bool)notificationData.Data[NotificationDataIDs.Failure];
            bool critical = (bool)notificationData.Data[NotificationDataIDs.Critical];
            float damage = (float)notificationData.Data[NotificationDataIDs.NewHP] - (float)notificationData.Data[NotificationDataIDs.PreviousHP];
            damage = Mathf.Abs(damage);

            if (failure)
                grid[position].PromptFailed();
            else
            {
                if (critical)
                    grid[position].PromptCritical();
                grid[position].PromptDamage(damage);
            }

        }
    }

    private void ShowDefenseBattleAction(GameNotificationData notificationData)
    {
        if (notificationData != null)
        {
            Vector2Int position = (Vector2Int)notificationData.Data[NotificationDataIDs.CellPosition];
            bool shieldState = (bool)notificationData.Data[NotificationDataIDs.ShieldState];
            grid[position].ShowShield(shieldState);
        }
    }

    private void UpdateDefenseInBoard()
    {
        foreach (KeyValuePair<Vector2Int, GridCellUI> cell in grid)
        {
            bool positionGuarded = TurnExecutor.Instance.DefenseValueInPosition(cell.Key) > 0;
            grid[cell.Key].ShowShield(positionGuarded);
        }
    }

    private void ClearBoardEffects(GameNotificationData notificationData)
    {
        foreach (KeyValuePair<Vector2Int, GridCellUI> cell in grid)
        {
            bool positionGuarded = TurnExecutor.Instance.DefenseValueInPosition(cell.Key) > 0;
            grid[cell.Key].ShowShield(false);
        }
    }

    private void UpdateBossFieldOfView(GameNotificationData notificationData)
    {
        List<Vector2Int> fov = BattleManager.Instance.CharacterManagement.Boss.GetFieldOfView();

        foreach (KeyValuePair<Vector2Int, GridCellUI> cell in grid)
        {
            grid[cell.Key].ToggleFOV(fov.Contains(cell.Key));
        }
        BattleManager.Instance.BattleGrid.CalculateHidingPositions();
        UpdateHidingPositions(BattleManager.Instance.BattleGrid.HidingPositions);
    }

    private void UpdateHidingPositions(List<Vector2Int> positions = null)
    {
        foreach (KeyValuePair<Vector2Int, GridCellUI> cell in grid)
        {
            if (positions == null || !positions.Contains(cell.Key))
            {
                cell.Value.ToggleHiding(false);
                continue;
            }
            else
                cell.Value.ToggleHiding(true);
        }
    }
}
