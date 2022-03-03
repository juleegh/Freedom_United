
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AIC Boss Performed Failure")]
public class AIC_BossPerformedFailure : AICondition
{
    public override bool MeetsRequirement()
    {
        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.LastTurnRegisters)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionOwner) == TargetType.BossPart)
            {
                if (actionInfo.wasFailure)
                    return true;
            }
        }

        return false;
    }
}