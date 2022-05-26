using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Attack RPC With Specific Parts")]
public class AIAttackRPCWithSpecificParts : AITurnAction
{
    [SerializeField] private List<BossPartType> attackingParts;

    protected override bool CheckAndAddToPile()
    {
        return SelectToAttackWithParts();
    }

    private bool SelectToAttackWithParts()
    {
        CharacterID counterPart = BattleManager.Instance.CharacterManagement.BossConfig.CharacterCounterPart;
        Vector2Int counterPartPosition = BattleManager.Instance.CharacterManagement.Characters[counterPart].CurrentPosition;

        foreach (BossPartType partType in attackingParts)
        {
            if(BattleManager.Instance.BattleValues.BossPartIsDestroyed(partType))
            {
                continue;
            }
            
            BossPart part = BattleManager.Instance.CharacterManagement.Boss.Parts[partType];
            List<SetOfPositions> areasOfEffect = part.AreasOfEffect;

            foreach (SetOfPositions areaOfEffect in areasOfEffect)
            {
                List<Vector2Int> pivots = areaOfEffect.GetPositions(part.Position, part.Orientation);

                foreach (SetOfPositions attackShape in part.ShapesOfAttack)
                {
                    foreach (Vector2Int pivot in pivots)
                    {
                        List<Vector2Int> attackPositions = attackShape.GetRotatedDeltasWithPivot(pivot, part.Orientation);
                        if (attackPositions.Contains(counterPartPosition))
                        {
                            BossAttackInfo attackInfo = new BossAttackInfo();
                            attackInfo.pivotDelta = pivot - part.Position;
                            attackInfo.attackShape = attackShape;
                            AddAttackActionToPile(part, attackInfo);
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}
