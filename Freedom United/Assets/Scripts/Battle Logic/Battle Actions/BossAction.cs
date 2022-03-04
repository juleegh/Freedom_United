using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : ScheduledAction
{
    public BossPartType actionOwner;
    public override string ActionOwner { get { return actionOwner.ToString(); } }

    public Vector2Int position;
    public SetOfPositions deltaOfAction;

    public List<Vector2Int> PreviewPositions
    {
        get
        {
            if (actionType == BattleActionType.Defend)
                return deltaOfAction.Positions;

            Vector2Int currentPartPosition = BattleManager.Instance.CharacterManagement.Boss.Parts[actionOwner].Position;
            Vector2Int currentPartOrientation = BattleManager.Instance.CharacterManagement.Boss.Parts[actionOwner].Orientation;
            return deltaOfAction.GetPositions(currentPartPosition + position, currentPartOrientation);
        }
    }
}