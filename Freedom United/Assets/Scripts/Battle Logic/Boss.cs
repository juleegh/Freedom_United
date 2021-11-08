using UnityEngine;
using System.Collections.Generic;

public class Boss
{
    private Vector2Int position;
    private List<BossPart> parts;

    public Vector2Int Position { get { return position; } }
    public List<BossPart> Parts { get { return parts; } }

    public Boss(Vector2Int pos, BossConfig config)
    {
        position = pos;
        parts = new List<BossPart>();

        foreach (KeyValuePair<BossPartType, BossPartConfig> part in config.PartsList)
        {
            BossPart nextPart = new BossPart(part.Key, part.Value.Position, part.Value.Dimensions.x, part.Value.Dimensions.y);
            parts.Add(nextPart);
        }
    }
}