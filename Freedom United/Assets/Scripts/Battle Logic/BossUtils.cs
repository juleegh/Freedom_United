using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackInfo
{
    public Vector2Int pivotDelta;
    public SetOfPositions attackShape;
}

public static class BossUtils
{
    public static BossPart GetPartWhoCanAttackPosition(Vector2Int targetPosition)
    {
        foreach (KeyValuePair<BossPartType, BossPart> bossPart in BattleManager.Instance.CharacterManagement.Boss.Parts)
        {
            if (BattleManager.Instance.BattleValues.BossPartIsDestroyed(bossPart.Key))
                continue;

            BossAttackInfo areaOfEffect = GetAreaOfEffectForPosition(bossPart.Value, targetPosition);
            if (areaOfEffect != null)
                return bossPart.Value;
        }

        return null;
    }

    public static BossAttackInfo GetAreaOfEffectForPosition(BossPart bossPart, Vector2Int targetPosition)
    {
        BossPart part = BattleManager.Instance.CharacterManagement.Boss.Parts[bossPart.PartType];
        foreach (SetOfPositions areaOfEffect in part.AreasOfEffect)
        {
            List<Vector2Int> pivots = areaOfEffect.GetPositions(bossPart.Position, bossPart.Orientation);

            foreach (SetOfPositions attackShape in part.ShapesOfAttack)
            {
                foreach (Vector2Int pivot in pivots)
                {
                    List<Vector2Int> attackPositions = attackShape.GetRotatedDeltas(bossPart.Orientation);
                    foreach (Vector2Int attackPosition in attackPositions)
                    {
                        Vector2Int usedPosition = attackPosition + pivot;
                        if (usedPosition == targetPosition)
                        {
                            BossAttackInfo attackInfo = new BossAttackInfo();
                            attackInfo.attackShape = attackShape;
                            attackInfo.pivotDelta = pivot - bossPart.Position;
                            return attackInfo;
                        }
                    }
                }
            }
        }

        return null;
    }

    public static List<Vector2Int> GetPositionsOccupiedByPart(BossPartType bossPartType)
    {
        return BattleManager.Instance.CharacterManagement.Boss.GetPositionsOccupiedByPart(bossPartType);
    }

    public static Vector2Int GetOrientedTransformation(Vector2Int orientation, int x, int y)
    {
        Vector2Int xValue = orientation * y;
        Vector2Int yValue = new Vector2Int(orientation.y, -orientation.x) * x;
        return xValue + yValue;
    }

    public static bool PartCanAttackPosition(BossPartType partType, Vector2Int position)
    {
        BossPart bossPart = BattleManager.Instance.CharacterManagement.Boss.Parts[partType];

        if(BattleManager.Instance.BattleValues.BossPartIsDestroyed(partType))
        {
            return false;
        }

        List<SetOfPositions> areasOfEffect = BattleManager.Instance.CharacterManagement.Boss.Parts[partType].AreasOfEffect;

        foreach (SetOfPositions areaOfEffect in areasOfEffect)
        {
            if (areaOfEffect.PositionInsideArea(position, bossPart.Position, bossPart.Orientation))
            {
                return true;
            }
        }
        return false;
    }

    public static List<Vector2Int> GetEffectivePositions(Vector2Int position, Vector2Int orientation, Vector2Int pivotDelta, SetOfPositions attackShape)
    {
        List<Vector2Int> transformedPositions = new List<Vector2Int>();
        List<Vector2Int> rotatedShape = attackShape.GetRotatedDeltas(orientation);
        Vector2Int pivot = pivotDelta + position;

        foreach (Vector2Int pos in rotatedShape)
        {
            transformedPositions.Add(pos + pivot);
        }
        return transformedPositions;
    }

    public static bool CanBeDefended(BossPartType toDefend)
    {
        if(BattleManager.Instance.BattleValues.BossPartIsDestroyed(toDefend))
        {
            return false;
        }

        List<BossPartConfig> parts = new List<BossPartConfig>(BattleManager.Instance.CharacterManagement.BossConfig.PartsList.Values);

        foreach (BossPartConfig part in parts)
        {
            if (part.DefendedParts.Contains(toDefend))
            {
                return true;
            }
        }
        return false;
    }

    public static SetOfPositions GetDefendArea(BossPartConfig defendingPart)
    {
        List<Vector2Int> positionsToDefend = new List<Vector2Int>();

        foreach (BossPartType defendedPart in defendingPart.DefendedParts)
        {
            BossPartConfig partConfig = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[defendedPart];
            List<Vector2Int> partPositions = BossUtils.GetPositionsOccupiedByPart(partConfig.PartType);
            positionsToDefend.AddRange(partPositions);
        }

        SetOfPositions areaOfDefense = new SetOfPositions(positionsToDefend);
        return areaOfDefense;
    }
}