using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Attack Highest HP")]
public class AIAttackHighestHP : AITurnAction
{
    protected override void AddToActionPile()
    {
        bool result = SelectUnitHighestHP();
        if (!result)
            AddToPileDefault();
    }

    private bool SelectUnitHighestHP()
    {
        float highest = 0;
        Vector2Int positionToAttack = Vector2Int.zero;

        foreach (Character character in BattleManager.Instance.CharacterManagement.Characters.Values)
        {
            if (BattleManager.Instance.BattleValues.PartyHealth[character.CharacterID] > highest)
            {
                highest = BattleManager.Instance.BattleValues.PartyHealth[character.CharacterID];
                positionToAttack = character.CurrentPosition;
            }
        }

        BossPart attackingPart = BossUtils.GetPartWhoCanAttackPosition(positionToAttack);
        BossAttackInfo usedAreaOfEffect = null;

        if (attackingPart != null)
        {
            usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(attackingPart, positionToAttack);
            AddAttackActionToPile(attackingPart, usedAreaOfEffect);
            return true;
        }

        return false;
    }
}
