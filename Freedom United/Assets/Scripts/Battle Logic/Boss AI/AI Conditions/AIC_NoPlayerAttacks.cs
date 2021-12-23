using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AIC No Player Attacks")]
public class AIC_NoPlayerAttacks : AICondition
{
    public override bool MeetsRequirement()
    {
        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.Registers)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionOwner) == TargetType.Character)
            {
                if (actionInfo.actionType == BattleActionType.Attack)
                    return false;
            }
        }

        return true;
    }
}