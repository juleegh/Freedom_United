using UnityEngine;
using System.Collections.Generic;

public class Boss
{
    private List<BossPart> parts;

    public List<BossPart> Parts { get { return parts; } }

    public Boss(BossConfig config)
    {
        parts = new List<BossPart>();

        foreach (KeyValuePair<BossPartType, BossPartConfig> part in config.PartsList)
        {
            BossPart nextPart = new BossPart(part.Key, part.Value.Position, part.Value.Dimensions.x, part.Value.Dimensions.y);
            parts.Add(nextPart);
        }
    }

    public bool OccupiesPosition(int x, int y)
    {
        foreach (BossPart part in parts)
        {
            if (part.OccupiesPosition(x, y))
                return true;

        }

        return false;
    }
}