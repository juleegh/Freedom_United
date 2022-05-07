using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AIC RPC On Sight")]
public class AICRPCOnSight : AICondition
{
    public override bool MeetsRequirement()
    {
        CharacterID counterPart = BattleManager.Instance.CharacterManagement.BossConfig.CharacterCounterPart;
        Vector2Int counterPosition = BattleManager.Instance.CharacterManagement.Characters[counterPart].CurrentPosition;
        bool isInFieldOfView = BattleManager.Instance.CharacterManagement.Boss.GetFieldOfView().Contains(counterPosition);
        bool isHidden = BattleManager.Instance.BattleGrid.HidingPositions.Contains(counterPosition);
        return BattleManager.Instance.BattleValues.IsAlive(counterPart) && isInFieldOfView && !isHidden;
    }
}
