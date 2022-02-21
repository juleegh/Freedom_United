using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BossUtils
{
    public static BossPart GetPartWhoCanAttackPosition(Vector2Int targetPosition)
    {
        foreach (KeyValuePair<BossPartType, BossPart> bossPart in BattleManager.Instance.CharacterManagement.Boss.Parts)
        {
            if (BattleManager.Instance.BattleValues.BossPartIsDestroyed(bossPart.Key))
                continue;

            List<SetOfPositions> areasOfEffect = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[bossPart.Key].AreasOfEffect;
            foreach (SetOfPositions areaOfEffect in areasOfEffect)
            {
                if (areaOfEffect.PositionInsideArea(targetPosition, bossPart.Value.Position, bossPart.Value.Orientation))
                    return BattleManager.Instance.CharacterManagement.Boss.Parts[bossPart.Key];
            }
        }

        return null;
    }

    public static SetOfPositions GetAreaOfEffectForPosition(BossPart bossPart, Vector2Int targetPosition)
    {
        BossPartConfig config = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[bossPart.PartType];
        foreach (SetOfPositions areaOfEffect in config.AreasOfEffect)
        {
            if (areaOfEffect.PositionInsideArea(targetPosition, bossPart.Position, bossPart.Orientation))
                return areaOfEffect;
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
}