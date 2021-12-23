using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIC Boss HP Below Half")]
public class AIC_BossHPBelowHalf : AICondition
{
    public override bool MeetsRequirement()
    {
        return BattleManager.Instance.BattleValues.BossHealth < BattleManager.Instance.CharacterManagement.BossConfig.BaseHealth * 0.5f;
    }
}