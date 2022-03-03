using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AIC Aware Of Anybody")]
public class AICAwareOfCharacters : AICondition
{
    public override bool MeetsRequirement()
    {
        foreach (CharacterID character in BattleManager.Instance.CharacterManagement.Characters.Keys)
        {
            if (TurnBlackBoard.Instance.IsAwareOfCharacter(character))
                return true;
        }
        return false;
    }
}
