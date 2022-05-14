
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

    private BossPartType GetWeightedRandom(List<BossPartType> canAttack, float sum)
    {
        float randomValue = UnityEngine.Random.Range(0f, sum);
        float newSum = 0;

        foreach (KeyValuePair<BossPartType, float> part in probabilities)
        {
            if (!canAttack.Contains(part.Key))
                continue;

            if (part.Value + newSum >= randomValue)
                return part.Key;

            newSum += part.Value;
        }

        return BossPartType.WrathBody;
    }

    private bool AttackRandomCharacter()
    {
        float chanceSum = 0;
        List<BossPartType> canAttack = new List<BossPartType>();

        foreach (KeyValuePair<BossPartType, float> part in probabilities)
        {
            List<Character> characters = BattleManager.Instance.CharacterManagement.Characters.Values.ToList();
            while (characters.Count > 0)
            {
                Character character = characters[UnityEngine.Random.Range(0, characters.Count)];
                characters.Remove(character);

                if (TurnBlackBoard.Instance.IsAwareOfCharacter(character.CharacterID) && BattleManager.Instance.BattleValues.IsAlive(character.CharacterID))
                {
                    if (BossUtils.PartCanAttackPosition(part.Key, character.CurrentPosition))
                    {
                        canAttack.Add(part.Key);
                        chanceSum += part.Value;
                        break;
                    }
                }
            }
        }

        BossPartType randomAttacker = GetWeightedRandom(canAttack, chanceSum);

        List<Character> allCharacters = BattleManager.Instance.CharacterManagement.Characters.Values.ToList();
        while (allCharacters.Count > 0)
        {
            Character character = allCharacters[UnityEngine.Random.Range(0, allCharacters.Count)];
            allCharacters.Remove(character);

            if (TurnBlackBoard.Instance.IsAwareOfCharacter(character.CharacterID) && BattleManager.Instance.BattleValues.IsAlive(character.CharacterID))
            {
                if (BossUtils.PartCanAttackPosition(randomAttacker, character.CurrentPosition))
                {
                    BossPart bossPart = BattleManager.Instance.CharacterManagement.Boss.Parts[randomAttacker];
                    BossAttackInfo usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(bossPart, character.CurrentPosition);
                    AddAttackActionToPile(bossPart, usedAreaOfEffect);
                    return true;
                }
            }
        }

        return false;
    }
}
