using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossVisuals : MonoBehaviour
{
    [SerializeField] private BossConfig bossConfig;
    private Dictionary<BossPartType, BossPartVisuals> partVisuals;

    [ContextMenu("Load")]
    public void Load()
    {
        partVisuals = new Dictionary<BossPartType, BossPartVisuals>();
        List<BossPartConfig> bossParts = bossConfig.PartsList.Values.ToList();
        BossPartVisuals[] bossPartVisuals = GetComponentsInChildren<BossPartVisuals>();

        for (int i = 0; i < bossConfig.PartsList.Count; i++)
        {
            BossPartConfig part = bossParts[i];
            BossPartVisuals nextPart = bossPartVisuals[i];
            nextPart.Paint(part);
            partVisuals[part.PartType] = nextPart;
        }
    }

    public Vector2Int GetPositionByPart(BossPartType partType)
    {
        BossPartVisuals part = partVisuals[partType];
        Vector3Int roundedPos = Vector3Int.RoundToInt(part.transform.position);
        return new Vector2Int(roundedPos.x, roundedPos.z);
    }

    public List<SetOfPositions> GetAreasOfEffectByPart(BossPartType partType)
    {
        BossPartVisuals part = partVisuals[partType];
        return part.LoadAreasOfEffect();
    }

    public List<SetOfPositions> GetShapesOfAttackByPart(BossPartType partType)
    {
        BossPartVisuals part = partVisuals[partType];
        return part.LoadShapesOfAttack();
    }

    public void Refresh(Boss boss)
    {
        foreach (KeyValuePair<BossPartType, BossPartVisuals> part in partVisuals)
        {
            BossPartVisuals nextPart = part.Value;
            BossPart partData = boss.Parts[part.Key];
            nextPart.Refresh(partData);
        }
    }

    public void PaintDeath(BossPartType death)
    {
        partVisuals[death].PaintDeath();
    }

    public void RemovePart(BossPartType partType)
    {
        partVisuals[partType].gameObject.SetActive(false);
    }
}
