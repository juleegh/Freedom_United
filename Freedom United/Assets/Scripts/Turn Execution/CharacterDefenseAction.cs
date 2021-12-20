using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterDefenseAction : ExecutingAction
{
    private CharacterID defendingCharacter;
    private List<Vector2Int> defendedPositions;
    private Vector2Int defenseDelta;

    private float defenseProvided;
    public float DefenseProvided { get { return defenseProvided; } }

    public CharacterDefenseAction(AllyAction scheduledAction) : base(scheduledAction)
    {
        defendingCharacter = scheduledAction.actionOwner;
        defenseDelta = scheduledAction.position;
        defenseProvided = BattleManager.Instance.PartyStats.Stats[scheduledAction.actionOwner].BaseDefense;
        defendedPositions = new List<Vector2Int>();

        if (defenseDelta != Vector2Int.zero)
            defenseProvided *= BattleGridUtils.DefenseSplitFactor;
    }

    public override void Execute()
    {
        defendedPositions.Add(defenseDelta);
        if (defenseDelta != Vector2Int.zero)
            defendedPositions.Add(Vector2Int.zero);
    }

    public bool PositionIsDefended(Vector2Int position)
    {
        return defendedPositions.Contains(position - BattleManager.Instance.CharacterManagement.Characters[defendingCharacter].CurrentPosition);
    }
}