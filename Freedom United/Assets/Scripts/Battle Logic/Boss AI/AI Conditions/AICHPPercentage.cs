using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AIC HP On Percentage")]
public class AICHPPercentage : AICondition
{
    [SerializeField] private float minPercentage;
    [SerializeField] private float maxPercentage;

    public override bool MeetsRequirement()
    {
        float currentPercentage = BattleManager.Instance.BattleValues.BossHealth / BattleManager.Instance.BattleValues.BaseBossHealth;
        return currentPercentage >= minPercentage && currentPercentage <= maxPercentage;
    }
}
