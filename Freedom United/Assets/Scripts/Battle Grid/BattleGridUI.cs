using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGridUI : MonoBehaviour, NotificationsListener
{
    private static BattleGridUI instance;
    public static BattleGridUI Instance { get { return instance; } }

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private float cellDistance;
    [SerializeField] private Transform cellsContainer;

    Dictionary<Vector2Int, GridCellUI> grid;
    Dictionary<CharacterID, CharacterVisuals> characters;
    BossVisuals bossVisuals;
    private float charactersHeightOnBoard = 0.02f;


    public void ConfigureComponent()
    {
        instance = this;
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleLoaded, InitializeGrid);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.CharacterMoved, RefreshCharacters);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.RecentDeath, RefreshDeaths);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.AttackWasExecuted, ShowAttackBattleAction);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.DefenseWasExecuted, ShowDefenseBattleAction);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.TurnEndedExecution, ClearBoardEffects);
    }

    private void InitializeGrid(GameNotificationData notificationData)
    {
        grid = new Dictionary<Vector2Int, GridCellUI>();

        for (int row = 0; row < BattleManager.Instance.BattleGrid.Height; row++)
        {
            for (int column = 0; column < BattleManager.Instance.BattleGrid.Width; column++)
            {
                Vector2Int position = new Vector2Int(column, row);
                Vector3 positionInWorld = new Vector3(column * cellDistance, 0, row * cellDistance);

                GridCellUI newGridCell = Instantiate(cellPrefab).GetComponent<GridCellUI>();
                newGridCell.Refresh(BattleManager.Instance.BattleGrid.GetInPosition(column, row).CellType);
                grid.Add(position, newGridCell);
                newGridCell.transform.SetParent(cellsContainer);
                newGridCell.transform.position = positionInWorld;
            }
        }

        characters = new Dictionary<CharacterID, CharacterVisuals>();

        foreach (KeyValuePair<CharacterID, Character> character in BattleManager.Instance.CharacterManagement.Characters)
        {
            CharacterVisuals characterVisuals = Instantiate(characterPrefab).GetComponent<CharacterVisuals>();
            characterVisuals.transform.SetParent(cellsContainer);
            characterVisuals.transform.position = BattleGridUtils.TranslatedPosition(character.Value.CurrentPosition, charactersHeightOnBoard);
            characterVisuals.Paint(character.Key);
            characters.Add(character.Key, characterVisuals);
        }

        bossVisuals = Instantiate(bossPrefab).GetComponent<BossVisuals>();
        bossVisuals.transform.SetParent(cellsContainer);
        bossVisuals.transform.position = BattleGridUtils.TranslatedPosition(Vector2Int.zero, charactersHeightOnBoard);
        bossVisuals.Paint(BattleManager.Instance.CharacterManagement.Boss);
    }

    private void RefreshCharacters(GameNotificationData notificationData)
    {
        foreach (KeyValuePair<CharacterID, Character> character in BattleManager.Instance.CharacterManagement.Characters)
        {
            CharacterVisuals characterVisuals = characters[character.Key];
            characterVisuals.transform.position = BattleGridUtils.TranslatedPosition(character.Value.CurrentPosition, charactersHeightOnBoard);
        }
        UpdateDefenseInBoard();
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
                cell.Value.CleanRange();
            else
                cell.Value.PaintAsRange(actionType);
        }
    }

    private void ShowAttackBattleAction(GameNotificationData notificationData)
    {
        if (notificationData != null)
        {
            Vector2Int position = (Vector2Int)notificationData.Data[NotificationDataIDs.CellPosition];
            bool failure = (bool)notificationData.Data[NotificationDataIDs.Failure];
            float damage = (float)notificationData.Data[NotificationDataIDs.NewHP] - (float)notificationData.Data[NotificationDataIDs.PreviousHP];
            damage = Mathf.Abs(damage);

            if (failure)
                grid[position].PromptFailed();
            else
                grid[position].PromptDamage(damage);

        }
    }

    private void ShowDefenseBattleAction(GameNotificationData notificationData)
    {
        if (notificationData != null)
        {
            Vector2Int position = (Vector2Int)notificationData.Data[NotificationDataIDs.CellPosition];
            grid[position].ShowShield(true);
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
}
