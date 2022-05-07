using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Attack Random Obstacle")]
public class AIAttackRandomObstacle : AITurnAction
{
    protected override bool CheckAndAddToPile()
    {
        return AttackRandomObstacle();
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
                BossAttackInfo usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(attackingPart, randomPosition);
                AddAttackActionToPile(attackingPart, usedAreaOfEffect);
                return true;
            }
        }
        return false;
    }
}
