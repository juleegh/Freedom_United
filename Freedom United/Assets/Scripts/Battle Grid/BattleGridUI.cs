using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGridUI : MonoBehaviour
{
    private static BattleGridUI instance;
    public static BattleGridUI Instance { get { return instance; } }

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private float cellDistance;
    [SerializeField] private Transform cellsContainer;

    Dictionary<Vector2Int, GridCellUI> grid;

    void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        grid = new Dictionary<Vector2Int, GridCellUI>();

        for (int row = 0; row < BattleGrid.Instance.Height; row++)
        {
            for (int column = 0; column < BattleGrid.Instance.Width; column++)
            {
                Vector2Int position = new Vector2Int(column, row);
                Vector3 positionInWorld = new Vector3(column * cellDistance, 0, row * cellDistance);

                GridCellUI newGridCell = Instantiate(cellPrefab).GetComponent<GridCellUI>();
                newGridCell.Refresh(BattleGrid.Instance.GetInPosition(column, row).CellType);
                grid.Add(position, newGridCell);
                newGridCell.transform.SetParent(cellsContainer);
                newGridCell.transform.position = positionInWorld;
            }
        }
    }
}
