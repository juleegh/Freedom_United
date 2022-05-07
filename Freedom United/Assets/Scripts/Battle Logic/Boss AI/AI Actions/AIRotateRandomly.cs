using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Rotate Randomly")]
public class AIRotateRandomly : AITurnAction
{
    protected override bool CheckAndAddToPile()
    {
        return RotateRandomly();
    }

    private bool RotateRandomly()
    {
        List<Vector2Int> rotations = new List<Vector2Int>();
        rotations.Add(Vector2Int.up);
        rotations.Add(Vector2Int.down);
        rotations.Add(Vector2Int.left);
        rotations.Add(Vector2Int.right);
        rotations.Remove(BattleManager.Instance.CharacterManagement.Boss.Orientation);
        AddRotationActionToPile(BattleManager.Instance.CharacterManagement.Boss.Core, rotations[Random.Range(0, 3)]);
        return true;
    }
}
