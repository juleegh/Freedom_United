using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Rotate Towards Player")]
public class AIRotateTowardsPlayer : AITurnAction
{
    protected override bool CheckAndAddToPile()
    {
        return RotateToCharacter();
    }

    private bool RotateToCharacter()
    {
        Vector2Int bossPosition = BattleManager.Instance.CharacterManagement.Boss.Position;
        Vector2Int bossOrientation = BattleManager.Instance.CharacterManagement.Boss.Orientation;
        CharacterID characterID = BattleManager.Instance.CharacterManagement.BossConfig.CharacterCounterPart;
        Character character = BattleManager.Instance.CharacterManagement.Characters[characterID];

        if (!BattleManager.Instance.BattleValues.IsAlive(characterID))
            return false;

        Vector2Int position = character.CurrentPosition;

        if (position.x < bossPosition.x && bossOrientation != Vector2Int.left)
        {
            AddRotationActionToPile(BattleManager.Instance.CharacterManagement.Boss.Core, Vector2Int.left);
            return true;
        }
        else if (position.x > bossPosition.x && bossOrientation != Vector2Int.right)
        {
            AddRotationActionToPile(BattleManager.Instance.CharacterManagement.Boss.Core, Vector2Int.right);
            return true;
        }
        else if (position.y > bossPosition.y && bossOrientation != Vector2Int.up)
        {
            AddRotationActionToPile(BattleManager.Instance.CharacterManagement.Boss.Core, Vector2Int.up);
            return true;
        }
        else if (position.y < bossPosition.y && bossOrientation != Vector2Int.down)
        {
            AddRotationActionToPile(BattleManager.Instance.CharacterManagement.Boss.Core, Vector2Int.down);
            return true;
        }
        return false;
    }
}
