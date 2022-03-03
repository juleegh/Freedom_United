using UnityEngine;
using System.Collections.Generic;

public class Boss
{
    private Dictionary<BossPartType, BossPart> parts;
    public Dictionary<BossPartType, BossPart> Parts { get { return parts; } }
    public Vector2Int Orientation { get { return core.Orientation; } }
    public Vector2Int Position { get { return core.Position; } }
    public BossPart Core { get { return core; } }
    private BossPart core;
    private int fovDepth;
    private int fovWidth;

    public Boss(BossConfig config)
    {
        fovDepth = config.FoVDepth;
        fovWidth = config.FoVWidth;

        parts = new Dictionary<BossPartType, BossPart>();

        foreach (KeyValuePair<BossPartType, BossPartConfig> part in config.PartsList)
        {
            BossPart nextPart = new BossPart(part.Value);
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

    public void Rotate(Vector2Int orientation)
    {
        core.Rotate(core.GetCenterPosition(), orientation);
        foreach (BossPart part in parts.Values)
        {
            if (part.RotateWithBody)
                part.Rotate(core.GetCenterPosition(), orientation);
        }
        GameNotificationsManager.Instance.Notify(GameNotification.FieldOfViewChanged);
    }

    public List<Vector2Int> GetFieldOfView()
    {
        List<Vector2Int> FoV = new List<Vector2Int>();
        Vector2Int nosePosition = core.Position + BossUtils.GetOrientedTransformation(core.Orientation, -core.Width / 2, 0);

        Vector2Int verticaldelta = -core.Orientation;
        Vector2Int horizontaldelta = new Vector2Int(-core.Orientation.y, core.Orientation.x);

        for (int depth = 1; depth <= fovDepth; depth++)
        {
            for (int width = -fovWidth * depth; width <= fovWidth * depth; width++)
            {
                Vector2Int delta = horizontaldelta * width + verticaldelta * -depth;
                FoV.Add(nosePosition + delta);
            }
        }

        return FoV;
    }
}