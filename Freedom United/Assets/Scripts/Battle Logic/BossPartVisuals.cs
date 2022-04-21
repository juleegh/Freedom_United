using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPartVisuals : MonoBehaviour
{
    [SerializeField] private BossPartType partType;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private BossesAssets assets;
    [SerializeField] private DeathIcon deathIcon;

    public void Paint(BossPartConfig part)
    {
        gameObject.name = part.PartType.ToString();
        partType = part.PartType;
        body.sprite = assets.Parts[part.PartType];
        deathIcon.transform.localPosition = new Vector3(part.Dimensions.x / 2, part.Dimensions.y / 2, -0.05f);
        deathIcon.Clear();
    }

    public List<SetOfPositions> LoadAreasOfEffect()
    {
        List<SetOfPositions> areasOfEffect = new List<SetOfPositions>();
        AreaOfEffectMarker[] areas = GetComponentsInChildren<AreaOfEffectMarker>();
        foreach (AreaOfEffectMarker area in areas)
        {
            areasOfEffect.Add(area.GetPositions());
        }

        return areasOfEffect;
    }

    public List<SetOfPositions> LoadShapesOfAttack()
    {
        List<SetOfPositions> shapesOfAttack = new List<SetOfPositions>();
        ShapeOfAttackMarker[] areas = GetComponentsInChildren<ShapeOfAttackMarker>();
        foreach (ShapeOfAttackMarker area in areas)
        {
            shapesOfAttack.Add(area.GetPositions());
        }

        return shapesOfAttack;
    }

    public void Refresh(BossPart part)
    {
        Vector2Int position = part.GetWorldPosition();
        transform.localPosition = new Vector3(position.x, 0, position.y);
        transform.localEulerAngles = Vector3.right * 90 + TransfromRotation(part.Orientation);
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
