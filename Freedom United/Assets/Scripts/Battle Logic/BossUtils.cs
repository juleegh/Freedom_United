using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BossUtils
{
    public static BossPart GetPartWhoCanAttackPosition(Vector2Int targetPosition)
    {
        foreach (KeyValuePair<BossPartType, BossPartConfig> bossPart in BattleManager.Instance.CharacterManagement.BossConfig.PartsList)
        {
            if (BattleManager.Instance.BattleValues.BossPartsHealth[bossPart.Key] <= 0 && !bossPart.Value.IsCore)
                continue;

            foreach (AreaOfEffect areaOfEffect in bossPart.Value.AreasOfEffect)
            {
                if (areaOfEffect.Positions.Contains(targetPosition))
                    return BattleManager.Instance.CharacterManagement.Boss.Parts[bossPart.Key];
            }
        }

        return null;
    }

    public static AreaOfEffect GetAreaOfEffectForPosition(BossPart bossPart, Vector2Int targetPosition)
    {

        BossPartConfig config = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[bossPart.PartType];
        foreach (AreaOfEffect areaOfEffect in config.AreasOfEffect)
        {
            if (areaOfEffect.Positions.Contains(targetPosition))
                return areaOfEffect;
        }

        return null;
    }

    public static List<Vector2Int> GetPositionsOccupiedByPart(BossPartConfig bossPart)
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        for (int column = 0; column < bossPart.Dimensions.x; column++)
        {
            for (int row = 0; row < bossPart.Dimensions.y; row++)
            {
                positions.Add(bossPart.Position + new Vector2Int(column, row));
            }
        }

        return positions;
    }
}