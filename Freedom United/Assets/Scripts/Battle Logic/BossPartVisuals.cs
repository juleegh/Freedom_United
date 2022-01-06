using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPartVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private BossesAssets assets;
    [SerializeField] private DeathIcon deathIcon;

    public void Paint(BossPart part, int xPosition, int yPosition)
    {
        body.sprite = assets.Parts[part.PartType];
        transform.localPosition = new Vector3(xPosition, 0, yPosition);
        deathIcon.transform.localPosition = new Vector3(part.Width / 2, part.Height / 2, -0.05f);
    }

    public void PaintDeath()
    {
        deathIcon.Died();
    }
}
