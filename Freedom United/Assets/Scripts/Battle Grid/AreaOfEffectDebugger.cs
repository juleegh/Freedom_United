using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectDebugger : MonoBehaviour
{
    [SerializeField] private Transform cellsContainer;
    [SerializeField] private BossPartType selectedPart;

    Dictionary<Vector2Int, GridCellUI> grid;

    void Awake()
    {
        grid = new Dictionary<Vector2Int, GridCellUI>();

        GridCellUI[] cells = cellsContainer.GetComponentsInChildren<GridCellUI>();
        foreach (GridCellUI cell in cells)
        {
            Vector3Int roundedPos = Vector3Int.RoundToInt(cell.transform.position);
            Vector2Int position = new Vector2Int(roundedPos.x, roundedPos.z);
            grid.Add(position, cell);
        }
    }

    [ContextMenu("Show Area Of Effect")]
    public void ShowPartAoE()
    {
        Clear();
        BossPart part = BattleManager.Instance.CharacterManagement.Boss.Parts[selectedPart];
        List<SetOfPositions> areasOfEffect = BattleManager.Instance.CharacterManagement.Boss.Parts[selectedPart].AreasOfEffect;

        foreach (Vector2Int position in areasOfEffect[0].Positions)
        {
            Vector2Int affectedPosition = BossUtils.GetOrientedTransformation(part.Orientation, position.x, position.y) + part.Position;
            if (!grid.ContainsKey(affectedPosition))
            {
                continue;
            }
            grid[affectedPosition].ToggleDebug(true);
        }
    }

    [ContextMenu("Show Shape Of Attack")]
    public void ShowPartShapeOfAttack()
    {
        Clear();
        BossPart part = BattleManager.Instance.CharacterManagement.Boss.Parts[selectedPart];
        List<SetOfPositions> shapesOfAttack = BattleManager.Instance.CharacterManagement.Boss.Parts[selectedPart].ShapesOfAttack;

        foreach (Vector2Int position in shapesOfAttack[0].Positions)
        {
            Vector2Int affectedPosition = BossUtils.GetOrientedTransformation(part.Orientation, position.x, position.y) + part.Position;
            if (!grid.ContainsKey(affectedPosition))
            {
                continue;
            }
            grid[affectedPosition].ToggleDebug(true);
        }
    }

    [ContextMenu("Clear All")]
    public void Clear()
    {
        foreach (GridCellUI cell in grid.Values)
        {
            cell.ToggleDebug(false);
        }
    }

}
