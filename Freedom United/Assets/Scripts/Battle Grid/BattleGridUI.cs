using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGridUI : MonoBehaviour
{
    private static BattleGridUI instance;
    public static BattleGridUI Instance { get { return instance; } }

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private float cellDistance;
    [SerializeField] private Transform cellsContainer;

    Dictionary<Vector2Int, GridCellUI> grid;
    Dictionary<CharacterID, CharacterVisuals> characters;

    void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
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
            characterVisuals.transform.position = new Vector3(character.Value.CurrentPosition.x, 0.1f, character.Value.CurrentPosition.y);
            characterVisuals.Paint(character.Key);
        }
    }
}
