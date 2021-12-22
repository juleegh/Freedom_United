using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Attack Last Who Attacked Me")]
public class AIAttackWhoAttackedMe : AIAttackAction
{
    protected override void AddToActionPile()
    {
        bool result = SelectWhoAttackedMe();
        if (!result)
            AddToPileDefault();
    }

    private bool SelectWhoAttackedMe()
    {
        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.Registers)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionOwner) == TargetType.Character)
            {
                if (BattleActionsUtils.GetTargetType(actionInfo.actionTarget) != TargetType.BossPart)
                    continue;

                CharacterID characterAttack = BattleGridUtils.GetCharacterID(actionInfo.actionOwner);
                Vector2Int positionToAttack = BattleManager.Instance.CharacterManagement.Characters[characterAttack].CurrentPosition;

                BossPart attackingPart = BossUtils.GetPartWhoCanAttackPosition(positionToAttack);
                AreaOfEffect usedAreaOfEffect = null;

                if (attackingPart != null)
                {
                    usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(attackingPart, positionToAttack);
                    AddAttackActionToPile(attackingPart, usedAreaOfEffect);
                    return true;
                }
            }
        }

        return false;
    }
}
