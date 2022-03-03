
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Attack Any Obstacle")]
public class AIAttackAnyObstacle : AITurnAction
{
    protected override void AddToActionPile()
    {
        bool result = AttackRandomObstacle();
    }

    private bool AttackRandomObstacle()
    {
        List<Vector2Int> obstaclePositions = BattleManager.Instance.BattleGrid.ObstaclePositions;
        while (obstaclePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, obstaclePositions.Count);
            Vector2Int randomPosition = obstaclePositions[randomIndex];
            obstaclePositions.Remove(randomPosition);

            BossPart attackingPart = BossUtils.GetPartWhoCanAttackPosition(randomPosition);
            if (attackingPart != null)
            {
                SetOfPositions usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(attackingPart, randomPosition);
                AddAttackActionToPile(attackingPart, usedAreaOfEffect);
                return true;
            }
        }

        return false;
    }
}
