using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Defend Last Critical")]
public class AIDefendPastCritical : AITurnAction
{
    protected override bool CheckAndAddToPile()
    {
        return SelectPreviouslyCriticallyAttack();
    }

    private bool SelectPreviouslyCriticallyAttack()
    {
        List<BossPartConfig> parts = new List<BossPartConfig>(BattleManager.Instance.CharacterManagement.BossConfig.PartsList.Values);

        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.LastTurnRegisters)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionOwner) != TargetType.Character || BattleActionsUtils.GetTargetType(actionInfo.actionTarget) != TargetType.BossPart)
                continue;

            BossPartType partToDefend = BattleGridUtils.GetBossPart(actionInfo.actionTarget);
            if (actionInfo.wasCritical)
            {
                if (BossUtils.CanBeDefended(partToDefend))
                {
                    foreach (BossPartConfig part in parts)
                    {
                        if (part.DefendedParts.Contains(partToDefend))
                        {
                            SetOfPositions defenseArea = BossUtils.GetDefendArea(part);
                            AddDefenseActionToPile(BattleManager.Instance.CharacterManagement.Boss.Parts[part.PartType], defenseArea);
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}
