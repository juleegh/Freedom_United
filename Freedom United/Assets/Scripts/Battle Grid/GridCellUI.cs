using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private FailPromptUI failPrompt;
    [SerializeField] private DamagePromptUI damagePrompt;

    CellType cellType;

    public void Refresh(CellType cellTypeDefined)
    {
        cellType = cellTypeDefined;
        background.color = cellTypeDefined != CellType.Available ? Color.gray : Color.black;
    }

    public void PaintAsRange(bool inRange)
    {
        background.color = cellType != CellType.Available ? Color.gray : Color.black;
        if (inRange)
            background.color = Color.red;
    }

    public void PromptFailed()
    {
        failPrompt.ShowFailure();
    }

    public void PromptDamage(float damageTaken)
    {
        damagePrompt.ShowDamage(damageTaken);
    }
}
