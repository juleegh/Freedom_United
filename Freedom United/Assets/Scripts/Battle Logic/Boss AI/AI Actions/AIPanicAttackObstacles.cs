using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Panic Attack Obstacles")]
public class AIPanicAttackObstacles : AITurnAction
{
    protected override void AddToActionPile()
    {
        bool rotatedBody = false;
        bool rotatedEverything = false;

        while (!BattleManager.Instance.ActionPile.BossReachedLimit)
        {
            if (WinCoinToss() && !rotatedBody)
            {
                RotateBody();
                rotatedBody = true;
            }
            else if (WinCoinToss() && !rotatedEverything)
            {
                RotateEverything();
                rotatedEverything = false;
            }
            else
                AttackRandomObstacle();

        }
    }

    private bool WinCoinToss()
    {
        return Random.Range(0f, 1f) >= 0.5f;
    }

    private void AttackRandomObstacle()
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
                return;
            }
        }
    }

    private void RotateBody()
    {
        List<Vector2Int> orientations = new List<Vector2Int>();
        orientations.Add(Vector2Int.down);
        orientations.Add(Vector2Int.up);
        orientations.Add(Vector2Int.left);
        orientations.Add(Vector2Int.right);
        orientations.Remove(BattleManager.Instance.CharacterManagement.Boss.Orientation);
        BattleManager.Instance.CharacterManagement.Boss.Rotate(orientations[Random.Range(0, 3)]);
    }

    private void RotateEverything()
    {
        foreach (BossPart bossPart in BattleManager.Instance.CharacterManagement.Boss.Parts.Values)
        {
            if (bossPart.RotateWithBody)
                continue;

            List<Vector2Int> orientations = new List<Vector2Int>();
            orientations.Add(Vector2Int.down);
            orientations.Add(Vector2Int.up);
            orientations.Add(Vector2Int.left);
            orientations.Add(Vector2Int.right);
            orientations.Remove(bossPart.Orientation);
            bossPart.Rotate(bossPart.Position, orientations[Random.Range(0, 3)]);
        }
    }
}
