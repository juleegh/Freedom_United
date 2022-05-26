using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Defend Part with Low HP")]
public class AIDefendPartWithLowHP : AITurnAction
{
    [SerializeField] private float inDangerHPPercentage;
    [SerializeField] private float defenseChance;

    protected override bool CheckAndAddToPile()
    {
        if (DefenseCheck())
        {
            return SelectPartToDefend();
        }
        return false;
    }

    private bool DefenseCheck()
    {
        return Random.Range(0f, 1f) <= defenseChance;
    }

    private bool SelectPartToDefend()
    {
        List<BossPartConfig> parts = new List<BossPartConfig>(BattleManager.Instance.CharacterManagement.BossConfig.PartsList.Values);
        BossPartType partToDefend = BossPartType.RobotHead;
        bool shouldDefend = false;

        foreach (BossPartConfig part in parts)
        {
            float hpPercentage = BattleManager.Instance.BattleValues.BossPartsHealth[part.PartType] / part.BaseDurability;
            if (hpPercentage <= inDangerHPPercentage && BossUtils.CanBeDefended(part.PartType))
            {
                partToDefend = part.PartType;
                shouldDefend = true;
                break;
            }
        }

        if (shouldDefend)
        {
            foreach (BossPartConfig part in parts)
            {
                if(BattleManager.Instance.BattleValues.BossPartIsDestroyed(part.PartType))
                {
                    continue;
                }

                if (part.DefendedParts.Contains(partToDefend))
                {
                    SetOfPositions defenseArea = BossUtils.GetDefendArea(part);
                    AddDefenseActionToPile(BattleManager.Instance.CharacterManagement.Boss.Parts[part.PartType], defenseArea);
                    return true;
                }
            }
        }

        return false;
    }
}
