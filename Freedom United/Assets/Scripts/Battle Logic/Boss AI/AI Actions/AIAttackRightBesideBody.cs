using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Attack Right Beside Body")]
public class AIAttackRightBesideBody : AITurnAction
{
    [SerializeField] private int unitsToAttack;

    protected override bool CheckAndAddToPile()
    {
        return SelectToAttackInFront();
    }

    private bool SelectToAttackInFront()
    {
        BossPart core = BattleManager.Instance.CharacterManagement.Boss.Core;
        BossPartType coreType = core.PartType;
        BossPart config = BattleManager.Instance.CharacterManagement.Boss.Parts[coreType];
        List<SetOfPositions> areasOfEffect = config.AreasOfEffect;
        bool includesRPC = false;

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
                            if (character.CharacterID == BattleManager.Instance.CharacterManagement.BossConfig.CharacterCounterPart)
                                includesRPC = true;

                            if (possibleAttacks >= unitsToAttack && includesRPC)
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
