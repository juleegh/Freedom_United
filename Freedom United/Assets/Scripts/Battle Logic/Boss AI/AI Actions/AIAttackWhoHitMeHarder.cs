
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

//[CreateAssetMenu(fileName = "AI Attack Who Hit Me Harder")]
public class AIAttackWhoHitMeHarder : AITurnAction
{
    protected override bool CheckAndAddToPile()
    {
        return RevengeAttackCharacter();
    }

    private bool RevengeAttackCharacter()
    {
        List<DamageInfo> damages = GetCharacterDamages();

        foreach (DamageInfo damage in damages)
        {
            Character character = damage.character;

            if (TurnBlackBoard.Instance.IsAwareOfCharacter(character.CharacterID) && BattleManager.Instance.BattleValues.IsAlive(character.CharacterID))
            {
                BossPart attackingPart = BossUtils.GetPartWhoCanAttackPosition(character.CurrentPosition);
                if (attackingPart != null)
                {
                    BossAttackInfo usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(attackingPart, character.CurrentPosition);
                    AddAttackActionToPile(attackingPart, usedAreaOfEffect);
                    return true;
                }
            }
        }

        return false;
    }

    private List<DamageInfo> GetCharacterDamages()
    {
        List<DamageInfo> damages = new List<DamageInfo>();

        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.LastTurnRegisters)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionOwner) != TargetType.Character)
                continue;

            if (BattleActionsUtils.GetTargetType(actionInfo.actionTarget) != TargetType.BossPart)
                continue;

            if (actionInfo.actionType != BattleActionType.Attack)
                continue;

            CharacterID characterID = BattleGridUtils.GetCharacterID(actionInfo.actionOwner);
            Character character = BattleManager.Instance.CharacterManagement.Characters[characterID];
            float damageDone = actionInfo.previousTargetHP - actionInfo.newTargetHP;

            damages.Add(new DamageInfo(character, damageDone));
        }

        return damages.OrderBy(o => (-o.damage)).ToList();
    }

    private class DamageInfo
    {
        public DamageInfo(Character c, float d)
        {
            character = c;
            damage = d;
        }
        public Character character;
        public float damage;
    }
}
