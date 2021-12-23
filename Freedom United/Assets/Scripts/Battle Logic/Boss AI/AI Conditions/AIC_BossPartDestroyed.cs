
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AIC Boss Part Destroyed")]
public class AIC_BossPartDestroyed : AICondition
{
    public override bool MeetsRequirement()
    {
        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.Registers)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionTarget) == TargetType.BossPart)
            {
                if (actionInfo.previousTargetHP > 0f && actionInfo.newTargetHP <= 0f)
                    return true;
            }
        }

        return false;
    }
}