using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AIC Counter Part HP Below Half")]
public class AIC_CounterPartHPHitZero : AICondition
{
    public override bool MeetsRequirement()
    {
        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.LastTurnRegisters)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionTarget) == TargetType.Character)
            {
                if (BattleGridUtils.GetCharacterID(actionInfo.actionTarget) != BattleManager.Instance.CharacterManagement.BossConfig.CharacterCounterPart)
                    continue;

                if (actionInfo.previousTargetHP > 0f && actionInfo.newTargetHP <= 0f)
                    return true;
            }
        }

        return false;
    }
}