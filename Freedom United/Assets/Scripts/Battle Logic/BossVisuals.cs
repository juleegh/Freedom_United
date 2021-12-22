using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVisuals : MonoBehaviour
{
    [SerializeField] private GameObject partPrefab;

    public void Paint(Boss boss)
    {
        foreach (BossPart part in boss.Parts.Values)
        {
            BossPartVisuals nextPart = Instantiate(partPrefab).GetComponent<BossPartVisuals>();
            nextPart.transform.SetParent(this.transform);
            nextPart.Paint(part.PartType, part.Position.x, part.Position.y);
        }
    }
}
