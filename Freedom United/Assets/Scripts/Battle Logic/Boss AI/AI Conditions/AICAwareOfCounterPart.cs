using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AIC Aware Of Counter Part")]
public class AICAwareOfCounterPart : AICondition
{
    public override bool MeetsRequirement()
    {
        CharacterID counterPart = BattleManager.Instance.CharacterManagement.BossConfig.CharacterCounterPart;
        return BattleManager.Instance.BattleValues.IsAlive(counterPart) && TurnBlackBoard.Instance.IsAwareOfCharacter(counterPart);
    }
}