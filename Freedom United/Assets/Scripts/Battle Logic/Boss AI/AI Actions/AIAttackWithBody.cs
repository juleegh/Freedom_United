using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Attack With Body")]
public class AIAttackWithBody : AITurnAction
{
    [SerializeField] private int unitsToAttack;
    [SerializeField] private bool fallbackToRandom;

    protected override void AddToActionPile()
    {
        bool result = SelectToAttackInFront();
        if (!result && fallbackToRandom)
            SelectRandomTarget();
    }

    private bool SelectToAttackInFront()
    {
        int possibleAttacks = 0;
        BossPart core = BattleManager.Instance.CharacterManagement.Boss.Core;
        BossPartType coreType = core.PartType;
        List<SetOfPositions> areasOfEffect = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[coreType].AreasOfEffect;

        foreach (SetOfPositions areaOfEffect in areasOfEffect)
        {
            foreach (Character character in BattleManager.Instance.CharacterManagement.Characters.Values)
            {
                Vector2Int characterPos = character.CurrentPosition;
                if (areaOfEffect.PositionInsideArea(characterPos, core.Position, core.Orientation))
                {
                    possibleAttacks++;
                }
            }

            if (possibleAttacks >= unitsToAttack)
            {
                AddAttackActionToPile(core, areaOfEffect);
                return true;
            }
        }

        return false;
    }
}
