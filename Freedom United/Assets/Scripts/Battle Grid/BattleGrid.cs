using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour
{
    private static BattleGrid instance;
    public static BattleGrid Instance { get { return instance; } }

    [SerializeField] private int width;
    [SerializeField] private int height;

    public int Width { get { return width; } }
    public int Height { get { return height; } }

    [SerializeField] private List<Vector2Int> availablePositions;

    private Dictionary<Vector2Int, GridCell> grid;

    void Awake()
    {
        instance = this;
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        grid = new Dictionary<Vector2Int, GridCell>();

        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                Vector2Int position = new Vector2Int(column, row);
                CellType cellType = availablePositions.Contains(position) ? CellType.Available : CellType.Obstacle;
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
}
