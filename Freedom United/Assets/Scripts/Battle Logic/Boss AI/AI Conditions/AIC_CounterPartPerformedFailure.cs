
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AIC Counter Part Performed Failure")]
public class AIC_CounterPartPerformedFailure : AICondition
{
    public override bool MeetsRequirement()
    {
        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.Registers)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionOwner) == TargetType.Character && BattleGridUtils.GetCharacterID(actionInfo.actionOwner) == BattleManager.Instance.CharacterManagement.BossConfig.CharacterCounterPart)
            {
                if (actionInfo.wasFailure)
                    return true;
            }
        }

        return false;
    }
}