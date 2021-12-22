using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Attack Lowest HP")]
public class AIAttackLowestHP : AIAttackAction
{
    protected override void AddToActionPile()
    {
        bool result = SelectUnitLowestHP();
        if (!result)
            AddToPileDefault();
    }

    private bool SelectUnitLowestHP()
    {
        float lowest = 0;
        Vector2Int positionToAttack = Vector2Int.zero;

        foreach (Character character in BattleManager.Instance.CharacterManagement.Characters.Values)
        {
            if (BattleManager.Instance.BattleValues.PartyHealth[character.CharacterID] <= 0)
                continue;

            if (BattleManager.Instance.BattleValues.PartyHealth[character.CharacterID] < lowest || lowest <= 0)
            {
                lowest = BattleManager.Instance.BattleValues.PartyHealth[character.CharacterID];
                positionToAttack = character.CurrentPosition;
            }
        }

        BossPart attackingPart = BossUtils.GetPartWhoCanAttackPosition(positionToAttack);
        AreaOfEffect usedAreaOfEffect = null;

        if (attackingPart != null)
        {
            usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(attackingPart, positionToAttack);
            AddAttackActionToPile(attackingPart, usedAreaOfEffect);
            return true;
        }

        return false;
    }
}
