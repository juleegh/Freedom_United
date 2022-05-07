
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

//[CreateAssetMenu(fileName = "AI Attack Random Position With Chances")]
public class AIAttackRandomPositionWithChances : AITurnAction
{
    [Serializable]
    public class ChancesDictionary : SerializableDictionaryBase<BossPartType, float> { }

    [SerializeField] private ChancesDictionary probabilities;

    protected override bool CheckAndAddToPile()
    {
        return AttackRandomPosition();
    }

    private bool PassCheck(BossPartType testPart)
    {
        return UnityEngine.Random.Range(0f, 1f) <= probabilities[testPart];
    }

    private bool AttackRandomPosition()
    {
        foreach (KeyValuePair<BossPartType, float> part in probabilities)
        {
            if (!PassCheck(part.Key))
            {
                continue;
            }

            BossPart bossPart = BattleManager.Instance.CharacterManagement.Boss.Parts[part.Key];
            List<SetOfPositions> areasOfEffect = BattleManager.Instance.CharacterManagement.Boss.Parts[part.Key].AreasOfEffect;
            SetOfPositions areaOfEffect = areasOfEffect[UnityEngine.Random.Range(0, areasOfEffect.Count)];

            List<SetOfPositions> shapesOfAttack = BattleManager.Instance.CharacterManagement.Boss.Parts[part.Key].ShapesOfAttack;
            SetOfPositions attackShape = shapesOfAttack[UnityEngine.Random.Range(0, shapesOfAttack.Count)];

            List<Vector2Int> pivots = areaOfEffect.GetPositions(bossPart.Position, bossPart.Orientation);
            Vector2Int pivot = pivots[UnityEngine.Random.Range(0, pivots.Count)];

            BossAttackInfo attackInfo = new BossAttackInfo();
            attackInfo.pivotDelta = pivot - bossPart.Position;
            attackInfo.attackShape = attackShape;
            AddAttackActionToPile(bossPart, attackInfo);
            return true;
        }

        return false;
    }
}
