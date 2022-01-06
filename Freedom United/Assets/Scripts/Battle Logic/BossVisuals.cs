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
            nextPart.Paint(part, part.Position.x, part.Position.y);
            partVisuals[part.PartType] = nextPart;
        }
    }

    public void PaintDeath(BossPartType death)
    {
        partVisuals[death].PaintDeath();
    }
}
