using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleGrid : MonoBehaviour, NotificationsListener
{
    [SerializeField] private float obstacleHP;
    [SerializeField] private int walkDistance;
    [SerializeField] private Transform cellsContainer;

    private Dictionary<BossPartType, PartObstacle> partObstacles;
    private Dictionary<Vector2Int, Obstacle> obstacles;
    private List<Vector2Int> gridPositions;

    private List<Vector2Int> positionsInRange;
    public List<Vector2Int> PositionsInRange { get { return positionsInRange; } }
    private List<Vector2Int> hidingPositions;
    public List<Vector2Int> HidingPositions { get { return hidingPositions; } }

    public void ConfigureComponent()
    {
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.DependenciesLoaded, InitializeGrid);
    }

    private void InitializeGrid(GameNotificationData notificationData)
    {
        gridPositions = new List<Vector2Int>();
        obstacles = new Dictionary<Vector2Int, Obstacle>();
        partObstacles = new Dictionary<BossPartType, PartObstacle>();
        positionsInRange = new List<Vector2Int>();
        hidingPositions = new List<Vector2Int>();

        GridCellUI[] cells = cellsContainer.GetComponentsInChildren<GridCellUI>();
        foreach (GridCellUI cell in cells)
        {
            Vector3Int roundedPos = Vector3Int.RoundToInt(cell.transform.position);
            Vector2Int position = new Vector2Int(roundedPos.x, roundedPos.z);
            gridPositions.Add(position);

            if (cell.IsObstacle)
            {
                obstacles.Add(position, new Obstacle(position, obstacleHP));
            }
        }
    }

    public void CalculateRange(AttackRange range, Vector2Int origin, bool includeOrigin)
    {
        positionsInRange.Clear();
        if (includeOrigin)
            positionsInRange.Add(origin);
        int extent = BattleGridUtils.GetRangeConversion(range);

        for (int i = 1; i <= extent; i++)
        {
            if (gridPositions.Contains(origin + Vector2Int.right * i))
                positionsInRange.Add(origin + Vector2Int.right * i);
            if (gridPositions.Contains(origin + Vector2Int.up * i))
                positionsInRange.Add(origin + Vector2Int.up * i);
            if (gridPositions.Contains(origin + Vector2Int.left * i))
                positionsInRange.Add(origin + Vector2Int.left * i);
            if (gridPositions.Contains(origin + Vector2Int.down * i))
                positionsInRange.Add(origin + Vector2Int.down * i);
        }
    }

    public void CalculateRange(Vector2Int origin)
    {
        positionsInRange.Clear();

        for (int column = -walkDistance; column <= walkDistance; column++)
        {
            int diagonal = walkDistance - Mathf.Abs(column);
            for (int row = -diagonal; row <= diagonal; row++)
            {
                Vector2Int position = new Vector2Int(column, row) + origin;

                if (position == origin || obstacles.ContainsKey(position))
                    continue;

                if (!gridPositions.Contains(position))
                    continue;

                if (BattleManager.Instance.CharacterManagement.Boss.OccupiesPosition(position.x, position.y))
                    continue;

                positionsInRange.Add(position);
            }
        }
    }

    public bool IsInsideGrid(int x, int y)
    {
        return gridPositions.Contains(new Vector2Int(x, y));
    }

    public void CalculateHidingPositions()
    {
        hidingPositions.Clear();
        List<Vector2Int> bossFieldOfView = BattleManager.Instance.CharacterManagement.Boss.GetFieldOfView();
        List<Vector2Int> positionsToCheck = ObstaclePositions;
        Vector2Int bossOrientation = BattleManager.Instance.CharacterManagement.Boss.Orientation;
        Vector2Int bossPosition = BattleManager.Instance.CharacterManagement.Boss.Position;

        foreach (Vector2Int position in positionsToCheck)
        {
            Vector2Int hidingPosition = position + bossOrientation;

            if (!IsInsideGrid(hidingPosition.x, hidingPosition.y))
                continue;

            if (!bossFieldOfView.Contains(hidingPosition))
                continue;

            if (positionsToCheck.Contains(hidingPosition))
                continue;
            hidingPositions.Add(hidingPosition);
        }
    }

    public CellType GetInPosition(int x, int y)
    {
        Vector2Int position = new Vector2Int(x, y);
        return obstacles.ContainsKey(position) ? CellType.Obstacle : CellType.Available;
    }

    public float GetObstacleHP(Vector2Int position)
    {
        if (obstacles.ContainsKey(position))
            return obstacles[position].HP;

        foreach (BossPartType obstacle in partObstacles.Keys)
        {
            List<Vector2Int> positions = BattleManager.Instance.CharacterManagement.Boss.Parts[obstacle].GetOccupiedPositions();
            if (positions.Contains(position))
                return partObstacles[obstacle].HP;
        }

        return 0;
    }

    public List<Vector2Int> ObstaclePositions
    {
        get
        {
            List<Vector2Int> obstaclePositions = obstacles.Keys.ToList();
            foreach (BossPartType obstacle in partObstacles.Keys)
            {
                List<Vector2Int> positions = BattleManager.Instance.CharacterManagement.Boss.Parts[obstacle].GetOccupiedPositions();
                obstaclePositions.AddRange(positions);
            }
            return obstaclePositions;
        }
    }

    public void AddPartObstacle(BossPartType partType)
    {
        partObstacles.Add(partType, new PartObstacle(Vector2Int.zero, partType, obstacleHP));
        GameNotificationsManager.Instance.Notify(GameNotification.FieldOfViewChanged);
    }

    public void HitObstacle(Vector2Int position, float damageTaken)
    {
        if (obstacles.ContainsKey(position))
        {
            obstacles[position].TakeDamage(damageTaken);
            if (obstacles[position].HP <= 0)
            {
                obstacles.Remove(position);
                GameNotificationData notificationData = new GameNotificationData();
                notificationData.Data[NotificationDataIDs.CellPosition] = position;
                GameNotificationsManager.Instance.Notify(GameNotification.ObstaclesStatsChanged, notificationData);
                GameNotificationsManager.Instance.Notify(GameNotification.FieldOfViewChanged);
            }
        }
        else
        {
            foreach (BossPartType obstacle in partObstacles.Keys)
            {
                List<Vector2Int> positions = BattleManager.Instance.CharacterManagement.Boss.Parts[obstacle].GetOccupiedPositions();
                if (positions.Contains(position))
                {
                    partObstacles[obstacle].TakeDamage(damageTaken);
                    if (partObstacles[obstacle].HP <= 0)
                    {
                        partObstacles.Remove(obstacle);
                        GameNotificationData notificationData = new GameNotificationData();
                        notificationData.Data[NotificationDataIDs.CellPosition] = position;
                        GameNotificationsManager.Instance.Notify(GameNotification.ObstaclesStatsChanged, notificationData);
                        GameNotificationsManager.Instance.Notify(GameNotification.FieldOfViewChanged);
                    }
                    break;
                }
            }
        }
    }
}
