using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour, NotificationsListener
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    public int Width { get { return width; } }
    public int Height { get { return height; } }

    [SerializeField] private List<Vector2Int> initialAvailablePositions;
    [SerializeField] private float obstacleHP;

    private Dictionary<Vector2Int, Obstacle> obstacles;
    public Dictionary<Vector2Int, Obstacle> Obstacles { get { return obstacles; } }
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
        obstacles = new Dictionary<Vector2Int, Obstacle>();
        positionsInRange = new List<Vector2Int>();
        hidingPositions = new List<Vector2Int>();

        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                Vector2Int position = new Vector2Int(column, row);
                if (initialAvailablePositions.Contains(position))
                    continue;

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
            if (origin.x + i < Width)
                positionsInRange.Add(origin + Vector2Int.right * i);
            if (origin.y + i < Height)
                positionsInRange.Add(origin + Vector2Int.up * i);
            if (origin.x - i >= 0)
                positionsInRange.Add(origin + Vector2Int.left * i);
            if (origin.y - i >= 0)
                positionsInRange.Add(origin + Vector2Int.down * i);
        }
    }

    public void CalculateRange(Vector2Int origin)
    {
        positionsInRange.Clear();

        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                Vector2Int position = new Vector2Int(column, row);

                if (position == origin || obstacles.ContainsKey(position))
                    continue;

                if (BattleManager.Instance.CharacterManagement.Boss.OccupiesPosition(position.x, position.y))
                    continue;

                positionsInRange.Add(position);
            }
        }
    }

    private bool IsInsideGrid(int x, int y)
    {
        return !(x < 0 || x >= height || y < 0 || y >= width);
    }

    public void CalculateHidingPositions()
    {
        hidingPositions.Clear();
        List<Vector2Int> bossFieldOfView = BattleManager.Instance.CharacterManagement.Boss.GetFieldOfView();
        Vector2Int bossOrientation = BattleManager.Instance.CharacterManagement.Boss.Orientation;
        Vector2Int bossPosition = BattleManager.Instance.CharacterManagement.Boss.Position;

        foreach (Vector2Int position in obstacles.Keys)
        {
            Vector2Int hidingPosition = position + bossOrientation;

            if (!IsInsideGrid(hidingPosition.x, hidingPosition.y))
                continue;

            if (obstacles.ContainsKey(hidingPosition))
                continue;

            if (!bossFieldOfView.Contains(hidingPosition))
                continue;

            hidingPositions.Add(hidingPosition);
        }
    }

    public CellType GetInPosition(int x, int y)
    {
        Vector2Int position = new Vector2Int(x, y);
        return obstacles.ContainsKey(position) ? CellType.Obstacle : CellType.Available;
    }

    public void HitObstacle(Vector2Int position, float damageTaken)
    {
        if (!obstacles.ContainsKey(position))
            return;

        obstacles[position].TakeDamage(damageTaken);
        if (obstacles[position].HP <= 0)
        {
            obstacles.Remove(position);
            GameNotificationData notificationData = new GameNotificationData();
            notificationData.Data[NotificationDataIDs.CellPosition] = position;
            GameNotificationsManager.Instance.Notify(GameNotification.ObstaclesStatsChanged, notificationData);
        }
    }
}
