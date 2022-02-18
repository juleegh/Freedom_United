using UnityEngine;
using System.Collections.Generic;

public class Boss
{
    private Dictionary<BossPartType, BossPart> parts;
    public Dictionary<BossPartType, BossPart> Parts { get { return parts; } }
    private BossPart core;

    public Boss(BossConfig config)
    {
        parts = new Dictionary<BossPartType, BossPart>();

        foreach (KeyValuePair<BossPartType, BossPartConfig> part in config.PartsList)
        {
            BossPart nextPart = new BossPart(part.Key, part.Value.InitialPosition, part.Value.Dimensions.x, part.Value.Dimensions.y, part.Value.RotatesWithBody);
            parts.Add(part.Key, nextPart);

            if (part.Value.IsCore)
                core = nextPart;
        }
    }

    public bool OccupiesPosition(int x, int y)
    {
        foreach (BossPart part in parts.Values)
        {
            if (part.OccupiesPosition(x, y))
                return true;

        }

        return false;
    }

    public List<Vector2Int> GetPositionsOccupiedByPart(BossPartType partType)
    {
        return parts[partType].GetOccupiedPositions();
    }

    public void Rotate()
    {
        Vector2Int orientation = new Vector2Int(core.Orientation.y, -core.Orientation.x);
        core.Rotate(core.GetCenterPosition(), orientation);
        foreach (BossPart part in parts.Values)
        {
            if (part.ShouldRotate)
                part.Rotate(core.GetCenterPosition(), orientation);
        }
    }
}