
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

//[CreateAssetMenu(fileName = "AI Random Attack With Chances")]
public class AIAttackRandomCharacterWithChances : AITurnAction
{
    [Serializable]
    public class ChancesDictionary : SerializableDictionaryBase<BossPartType, float> { }

    [SerializeField] private ChancesDictionary probabilities;

    protected override bool CheckAndAddToPile()
    {
        return AttackRandomCharacter();
    }

    private bool PassCheck(BossPartType testPart)
    {
        return UnityEngine.Random.Range(0f, 1f) <= probabilities[testPart];
    }

    private bool AttackRandomCharacter()
    {
        foreach (KeyValuePair<BossPartType, float> part in probabilities)
        {
            if (!PassCheck(part.Key))
            {
                continue;
            }

            List<Character> characters = BattleManager.Instance.CharacterManagement.Characters.Values.ToList();
            while (characters.Count > 0)
            {
                Character character = characters[UnityEngine.Random.Range(0, characters.Count)];
                characters.Remove(character);

                if (TurnBlackBoard.Instance.IsAwareOfCharacter(character.CharacterID) && BattleManager.Instance.BattleValues.IsAlive(character.CharacterID))
                {
                    if (BossUtils.PartCanAttackPosition(part.Key, character.CurrentPosition))
                    {
                        BossPart bossPart = BattleManager.Instance.CharacterManagement.Boss.Parts[part.Key];
                        BossAttackInfo usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(bossPart, character.CurrentPosition);
                        AddAttackActionToPile(bossPart, usedAreaOfEffect);
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
