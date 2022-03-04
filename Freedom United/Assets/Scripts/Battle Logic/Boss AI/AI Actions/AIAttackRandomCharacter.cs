using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "AI Attack Random Character")]
public class AIAttackRandomCharacter : AITurnAction
{
    protected override void AddToActionPile()
    {
        bool result = AttackRandomCharacter();
    }

    private bool AttackRandomCharacter()
    {
        List<CharacterID> characters = BattleManager.Instance.CharacterManagement.Characters.Keys.ToList();

        while (characters.Count > 0)
        {
            CharacterID characterToAttack = characters[Random.Range(0, characters.Count)];
            characters.Remove(characterToAttack);

            if (BattleManager.Instance.BattleValues.IsAlive(characterToAttack) && TurnBlackBoard.Instance.IsAwareOfCharacter(characterToAttack))
            {
                Vector2Int characterPosition = BattleManager.Instance.CharacterManagement.Characters[characterToAttack].CurrentPosition;
                List<BossPart> parts = BattleManager.Instance.CharacterManagement.Boss.Parts.Values.ToList();

                while (parts.Count > 0)
                {
                    BossPart attackingPart = parts[Random.Range(0, parts.Count)];
                    parts.Remove(attackingPart);
                    BossAttackInfo usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(attackingPart, characterPosition); ;

                    if (usedAreaOfEffect != null)
                    {
                        AddAttackActionToPile(attackingPart, usedAreaOfEffect);
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
