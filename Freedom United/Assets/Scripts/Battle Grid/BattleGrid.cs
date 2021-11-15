using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    public int Width { get { return width; } }
    public int Height { get { return height; } }

    [SerializeField] private List<Vector2Int> initialAvailablePositions;

    private Dictionary<Vector2Int, GridCell> grid;
    private List<Vector2Int> positionsInRange;
    public List<Vector2Int> PositionsInRange { get { return positionsInRange; } }

    void Awake()
    {
        InitializeGrid();
        positionsInRange = new List<Vector2Int>();
    }

    private void InitializeGrid()
    {
        grid = new Dictionary<Vector2Int, GridCell>();
        positionsInRange = new List<Vector2Int>();

        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                Vector2Int position = new Vector2Int(column, row);
                CellType cellType = initialAvailablePositions.Contains(position) ? CellType.Available : CellType.Obstacle;
                GridCell newGridCell = new GridCell(cellType);
                grid.Add(position, newGridCell);
            }
        }
    }

    public GridCell GetInPosition(int column, int row)
    {
        Vector2Int position = new Vector2Int(column, row);
        return grid[position];
    }

    public void CalculateRange(AttackRange range, Vector2Int origin)
    {
        positionsInRange.Clear();
        positionsInRange.Add(origin);
        int extent = BattleGridUtils.GetRangeConversion(range);

        for (int i = 1; i <= extent; i++)
        {
            if (origin.x + i < Width)
                positionsInRange.Add(origin + Vector2Int.right * i);
            if (origin.y + i < Height)
                positionsInRange.Add(origin + Vector2Int.up * i);
            if (origin.y - i >= 0)
                positionsInRange.Add(origin + Vector2Int.left * i);
            if (origin.y - i >= 0)
                positionsInRange.Add(origin + Vector2Int.down * i);
        }
    }
}
