
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AIC Has Not Rotated Recently")]
public class AICHasNotRotatedRecently : AICondition
{
    public override bool MeetsRequirement()
    {
        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.LastTurnRegisters)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionOwner) == TargetType.BossPart)
            {
                if (actionInfo.actionType == BattleActionType.Rotate)
                    return false;
            }
        }

        return true;
    }
}