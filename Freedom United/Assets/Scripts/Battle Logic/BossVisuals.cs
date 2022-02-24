using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVisuals : MonoBehaviour
{
    [SerializeField] private GameObject partPrefab;
    private Dictionary<BossPartType, BossPartVisuals> partVisuals;

    public void Paint(Boss boss)
    {
        partVisuals = new Dictionary<BossPartType, BossPartVisuals>();
        foreach (BossPart part in boss.Parts.Values)
        {
            BossPartVisuals nextPart = Instantiate(partPrefab).GetComponent<BossPartVisuals>();
            nextPart.transform.SetParent(this.transform);
            nextPart.Paint(part);
            partVisuals[part.PartType] = nextPart;
        }
    }

    public void Refresh(Boss boss)
    {
        foreach (KeyValuePair<BossPartType, BossPartVisuals> part in partVisuals)
        {
            BossPartVisuals nextPart = part.Value;
            BossPart partData = boss.Parts[part.Key];
            nextPart.Paint(partData);
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
