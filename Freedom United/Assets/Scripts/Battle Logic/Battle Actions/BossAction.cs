using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : ScheduledAction
{
    public BossPartType actionOwner;
    public override string ActionOwner { get { return actionOwner.ToString(); } }

    public AreaOfEffect areaOfEffect;

    public List<Vector2Int> PreviewPositions
    {
        get
        {
            if (actionType == BattleActionType.Defend)
                return areaOfEffect.Positions;

            Vector2Int currentPartPosition = BattleManager.Instance.CharacterManagement.Boss.Parts[actionOwner].Position;
            Vector2Int currentPartOrientation = BattleManager.Instance.CharacterManagement.Boss.Parts[actionOwner].Orientation;
            return areaOfEffect.GetPositions(currentPartPosition, currentPartOrientation);
        }
    }
}