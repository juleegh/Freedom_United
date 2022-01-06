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
        CharacterID characterAttack = CharacterID.Noma;
        Vector2Int positionToAttack = Vector2Int.zero;
        bool foundOne = false;

        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.Registers)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionOwner) == TargetType.Character)
            {
                if (BattleActionsUtils.GetTargetType(actionInfo.actionTarget) != TargetType.BossPart)
                    continue;

                CharacterID character = BattleGridUtils.GetCharacterID(actionInfo.actionOwner);
                if (!BattleManager.Instance.BattleValues.IsAlive(character))
                    continue;
                Vector2Int pos = BattleManager.Instance.CharacterManagement.Characters[character].CurrentPosition;

                BossPart attackingPart = BossUtils.GetPartWhoCanAttackPosition(pos);

                if (attackingPart != null)
                {
                    characterAttack = character;
                    positionToAttack = pos;
                    foundOne = true;
                }
            }
        }

        if (!foundOne)
            return false;

        BossPart partWillAttack = BossUtils.GetPartWhoCanAttackPosition(positionToAttack);

        if (partWillAttack != null)
        {
            AreaOfEffect usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(partWillAttack, positionToAttack);
            AddAttackActionToPile(partWillAttack, usedAreaOfEffect);
            return true;
        }

        return false;
    }
}
