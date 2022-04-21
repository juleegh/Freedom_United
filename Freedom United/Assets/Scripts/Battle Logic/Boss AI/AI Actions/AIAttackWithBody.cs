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
        BossPart core = BattleManager.Instance.CharacterManagement.Boss.Core;
        BossPartType coreType = core.PartType;
        BossPart config = BattleManager.Instance.CharacterManagement.Boss.Parts[coreType];
        List<SetOfPositions> areasOfEffect = config.AreasOfEffect;

        foreach (SetOfPositions areaOfEffect in areasOfEffect)
        {
            List<Vector2Int> pivots = areaOfEffect.GetPositions(core.Position, core.Orientation);

            foreach (SetOfPositions attackShape in config.ShapesOfAttack)
            {
                foreach (Vector2Int pivot in pivots)
                {
                    int possibleAttacks = 0;
                    List<Vector2Int> attackPositions = attackShape.GetRotatedDeltasWithPivot(pivot, core.Orientation);
                    foreach (Character character in BattleManager.Instance.CharacterManagement.Characters.Values)
                    {
                        Vector2Int characterPos = character.CurrentPosition;
                        if (attackPositions.Contains(characterPos))
                        {
                            possibleAttacks++;
                            if (possibleAttacks >= unitsToAttack)
                            {
                                BossAttackInfo attackInfo = new BossAttackInfo();
                                attackInfo.pivotDelta = pivot - core.Position;
                                attackInfo.attackShape = attackShape;
                                AddAttackActionToPile(core, attackInfo);
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
}
