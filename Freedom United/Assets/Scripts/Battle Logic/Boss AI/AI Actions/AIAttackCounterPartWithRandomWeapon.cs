using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//[CreateAssetMenu(fileName = "AI Attack Counter Part With Random Weapon")]
public class AIAttackCounterPartWithRandomWeapon : AITurnAction
{
    protected override void AddToActionPile()
    {
        bool result = AttackCounterPart();
    }

    private bool AttackCounterPart()
    {
        CharacterID counterPart = BattleManager.Instance.CharacterManagement.BossConfig.CharacterCounterPart;

        if (!BattleManager.Instance.BattleValues.IsAlive(counterPart))
            return false;

        Vector2Int counterPartPosition = BattleManager.Instance.CharacterManagement.Characters[counterPart].CurrentPosition;
        List<BossPart> parts = BattleManager.Instance.CharacterManagement.Boss.Parts.Values.ToList();

        while (parts.Count > 0)
        {
            BossPart attackingPart = parts[Random.Range(0, parts.Count)];
            parts.Remove(attackingPart);
            BossAttackInfo usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(attackingPart, counterPartPosition);

            if (usedAreaOfEffect != null)
            {
                AddAttackActionToPile(attackingPart, usedAreaOfEffect);
                return true;
            }
        }

        return false;
    }
}
