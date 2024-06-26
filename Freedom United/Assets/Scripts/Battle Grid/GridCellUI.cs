using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellUI : MonoBehaviour
{
    [SerializeField] private Sprite obstacle;
    [SerializeField] private Sprite empty;
    [SerializeField] private SpriteRenderer highlight;
    [SerializeField] private SpriteRenderer content;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private AttackInfoPrompt failPrompt;
    [SerializeField] private DamagePromptUI damagePrompt;
    [SerializeField] private RunPromptUI runPrompt;
    [SerializeField] private ShieldPromptUI shieldPrompt;
    [SerializeField] private DirectionPromptUI directionPrompt;
    [SerializeField] private FOVIndicator fovIndicator;
    [SerializeField] private CellDebugger debugger;

    [SerializeField] private Color highlightColor;
    [SerializeField] private Color attackColor;
    [SerializeField] private Color defenseColor;
    [SerializeField] private Color moveColor;

    CellType cellType;
    public bool IsObstacle { get { return background.sprite == obstacle; } }
    private Color transparent = new Color(0, 0, 0, 0);

    public void Refresh(CellType cellTypeDefined)
    {
        cellType = cellTypeDefined;
        background.sprite = cellTypeDefined != CellType.Available ? obstacle : empty;
        content.color = transparent;
    }

    [ContextMenu("Set As Empty")]
    private void SetAsEmpty()
    {
        Refresh(CellType.Available);
        ShowShield(false);
        runPrompt.Clear();
        failPrompt.Clear();
        damagePrompt.Clear();
        shieldPrompt.Clear();
        fovIndicator.ToggleFOV(false);
        ToggleHiding(false);
    }

    [ContextMenu("Set As Obstacle")]
    private void SetAsObstacle()
    {
        Refresh(CellType.Obstacle);
    }

    public void CleanRange()
    {
        content.color = transparent;
        directionPrompt.ClearPrompt();
    }

    public void PaintAsHighlight(bool highlighted)
    {
        if (highlighted)
        {
            highlight.color = highlightColor;
        }
        else
        {
            highlight.color = transparent;
        }
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
            case BattleActionType.Rotate:
                color = moveColor;
                break;
        }
        content.color = color;
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

    public void ToggleDebug(bool visible)
    {
        debugger.Toggle(visible);
    }

    public void ToggleDirection(Vector2Int direction)
    {
        //directionPrompt.SetupPrompt(direction);
    }
}
