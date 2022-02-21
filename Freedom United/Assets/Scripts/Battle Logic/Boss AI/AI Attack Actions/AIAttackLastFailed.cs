using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Retry Failed Attack")]
public class AIAttackLastFailed : AIAttackAction
{
    protected override void AddToActionPile()
    {
        bool result = SelectPreviouslyFailed();
        if (!result)
            AddToPileDefault();
    }

    private bool SelectPreviouslyFailed()
    {
        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.Registers)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionOwner) == TargetType.BossPart)
            {
                if (BattleActionsUtils.GetTargetType(actionInfo.actionTarget) != TargetType.Character)
                    continue;

                if (actionInfo.wasFailure)
                {
                    CharacterID characterAttack = BattleGridUtils.GetCharacterID(actionInfo.actionTarget);

                    if (!BattleManager.Instance.BattleValues.IsAlive(characterAttack))
                        continue;

                    Vector2Int positionToAttack = BattleManager.Instance.CharacterManagement.Characters[characterAttack].CurrentPosition;

                    BossPart attackingPart = BossUtils.GetPartWhoCanAttackPosition(positionToAttack);
                    SetOfPositions usedAreaOfEffect = null;

                    if (attackingPart != null)
                    {
                        usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(attackingPart, positionToAttack);
                        AddAttackActionToPile(attackingPart, usedAreaOfEffect);
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
