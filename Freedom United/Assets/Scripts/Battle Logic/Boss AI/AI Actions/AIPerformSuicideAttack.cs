using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Perform Suicide Attack")]
public class AIPerformSuicideAttack : AITurnAction
{
    protected override bool CheckAndAddToPile()
    {
        return PerformSuicideAttack();
    }

    private bool PerformSuicideAttack()
    {
        BossAction currentAction = new BossAction();
        currentAction.actionOwner = BattleManager.Instance.CharacterManagement.Boss.Core.PartType;
        currentAction.actionType = BattleActionType.SuicideAttack;
        currentAction.deltaOfAction = GetAttackedPositions();
        currentAction.speed = BattleManager.Instance.CharacterManagement.BossConfig.SuicideAttackSpeed;
        BattleManager.Instance.ActionPile.AddActionToPile(currentAction);
        Debug.LogWarning("-- Suicide Attack Action Added: " + GetType());

        return true;
    }

    private SetOfPositions GetAttackedPositions()
    {
        int amountOfCells = BattleManager.Instance.CharacterManagement.BossConfig.AmountOfSuicideCells;

        List<Vector2Int> selectedCells = new List<Vector2Int>();
        List<Vector2Int> availableCells = new List<Vector2Int>();
        availableCells.AddRange(BattleManager.Instance.BattleGrid.GridPositions);

        while (availableCells.Count > 0 && selectedCells.Count < amountOfCells)
        {
            Vector2Int randomCell = availableCells[Random.Range(0, availableCells.Count)];
            availableCells.Remove(randomCell);

            if (BattleManager.Instance.CharacterManagement.GetBossPartInPosition(randomCell) != null)
                continue;

            selectedCells.Add(randomCell);
        }

        SetOfPositions positions = new SetOfPositions(selectedCells);
        return positions;
    }
}
