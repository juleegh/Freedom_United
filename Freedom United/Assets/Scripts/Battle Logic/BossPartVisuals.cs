using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPartVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private BossesAssets assets;

    public void Paint(BossPartType partType, int xPosition, int yPosition)
    {
        body.sprite = assets.Parts[partType];
        transform.localPosition = new Vector3(xPosition, 0, yPosition);
    }
}
