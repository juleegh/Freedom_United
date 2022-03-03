using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AIC No Awareness")]
public class AICNoAwareness : AICondition
{
    public override bool MeetsRequirement()
    {
        foreach (CharacterID character in BattleManager.Instance.CharacterManagement.Characters.Keys)
        {
            if (TurnBlackBoard.Instance.IsAwareOfCharacter(character))
                return false;
        }
        return true;
    }
}
