using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Attack Lowest HP")]
public class AIAttackLowestHP : AITurnAction
{
    protected override void AddToActionPile()
    {
        bool result = SelectUnitLowestHP();
    }

    private bool SelectUnitLowestHP()
    {
        float lowest = 0;
        Vector2Int positionToAttack = Vector2Int.zero;
        bool found = false;

        foreach (Character character in BattleManager.Instance.CharacterManagement.Characters.Values)
        {
            if (!BattleManager.Instance.BattleValues.IsAlive(character.CharacterID) || !TurnBlackBoard.Instance.IsAwareOfCharacter(character.CharacterID))
                continue;

            BossPart attackPart = BossUtils.GetPartWhoCanAttackPosition(character.CurrentPosition);
            if (attackPart == null)
                continue;

            if (BattleManager.Instance.BattleValues.PartyHealth[character.CharacterID] < lowest || lowest <= 0)
            {
                lowest = BattleManager.Instance.BattleValues.PartyHealth[character.CharacterID];
                positionToAttack = character.CurrentPosition;
                found = true;
            }
        }

        if (!found)
            return found;

        BossPart attackingPart = BossUtils.GetPartWhoCanAttackPosition(positionToAttack);
        BossAttackInfo usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(attackingPart, positionToAttack);
        AddAttackActionToPile(attackingPart, usedAreaOfEffect);
        return true;
    }
}
