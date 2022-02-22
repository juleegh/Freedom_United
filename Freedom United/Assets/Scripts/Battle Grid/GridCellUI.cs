using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer highlight;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private AttackInfoPrompt failPrompt;
    [SerializeField] private DamagePromptUI damagePrompt;
    [SerializeField] private RunPromptUI runPrompt;
    [SerializeField] private ShieldPromptUI shieldPrompt;
    [SerializeField] private FOVIndicator fovIndicator;

    [SerializeField] private Color attackColor;
    [SerializeField] private Color defenseColor;
    [SerializeField] private Color moveColor;

    CellType cellType;
    private Color transparent = new Color(0, 0, 0, 0);

    public void Refresh(CellType cellTypeDefined)
    {
        cellType = cellTypeDefined;
        background.color = cellTypeDefined != CellType.Available ? Color.gray : Color.black;
        highlight.color = transparent;
    }

    public void CleanRange()
    {
        highlight.color = transparent;
    }

    public void PaintAsRange(BattleActionType rangeType)
    {
        Color color = attackColor;
        switch (rangeType)
        {
            case BattleActionType.Attack:
                color = attackColor;
                break;
            case BattleActionType.Defend:
                color = defenseColor;
                break;
            case BattleActionType.MoveSafely:
            case BattleActionType.MoveFast:
                color = moveColor;
                break;
        }
        highlight.color = color;
    }

    public void PromptCritical()
    {
        failPrompt.ShowCritical();
    }

    public void PromptFailed()
    {
        failPrompt.ShowFailure();
    }

    public void PromptDamage(float damageTaken)
    {
        damagePrompt.ShowDamage(damageTaken);
    }

    public void PromptMove()
    {
        runPrompt.ShowRun();
    }

    public void ShowShield(bool guarded)
    {
        shieldPrompt.ShowShield(guarded);
    }

    public void ToggleFOV(bool visible)
    {
        fovIndicator.ToggleFOV(visible);
    }

    public void ToggleHiding(bool visible)
    {
        fovIndicator.ToggleHiding(visible);
    }
}
