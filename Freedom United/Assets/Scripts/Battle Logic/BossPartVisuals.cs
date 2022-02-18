using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPartVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private BossesAssets assets;
    [SerializeField] private DeathIcon deathIcon;

    public void Paint(BossPart part)
    {
        body.sprite = assets.Parts[part.PartType];
        Vector2Int position = part.GetWorldPosition();
        transform.localPosition = new Vector3(position.x, 0, position.y);
        transform.localEulerAngles = Vector3.right * 90 + TransfromRotation(part.Orientation);
        deathIcon.transform.localPosition = new Vector3(part.Width / 2, part.Height / 2, -0.05f);
    }

    private Vector3 TransfromRotation(Vector2Int orientation)
    {
        if (orientation == Vector2Int.down)
            return Vector3.zero;
        if (orientation == Vector2Int.left)
            return Vector3.up * 90;
        if (orientation == Vector2Int.right)
            return Vector3.up * -90;
        if (orientation == Vector2Int.up)
            return Vector3.up * 180;
        return Vector3.zero;
    }

    public void PaintDeath()
    {
        deathIcon.Died();
    }
}
