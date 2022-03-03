
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

//[CreateAssetMenu(fileName = "AI Random Attack With Chances")]
public class AIRandomAttackWithChances : AITurnAction
{
    [Serializable]
    public class ChancesDictionary : SerializableDictionaryBase<BossPartType, float> { }

    [SerializeField] private ChancesDictionary probabilities;

    protected override void AddToActionPile()
    {
        bool result = AttackRandomCharacter();
        //if (!result)
        //  RotateToObstacle();
    }

    private bool AttackRandomCharacter()
    {
        List<CharacterID> knownCharacters = new List<CharacterID>();
        foreach (Character character in BattleManager.Instance.CharacterManagement.Characters.Values)
        {
            if (TurnBlackBoard.Instance.IsAwareOfCharacter(character.CharacterID) && BattleManager.Instance.BattleValues.IsAlive(character.CharacterID))
            {
                if (BossUtils.GetPartWhoCanAttackPosition(character.CurrentPosition) != null)
                {
                    knownCharacters.Add(character.CharacterID);
                }
            }
        }

        if (knownCharacters.Count == 0)
            return false;

        CharacterID characterToAttack = knownCharacters[UnityEngine.Random.Range(0, knownCharacters.Count)];
        Vector2Int characterPosition = BattleManager.Instance.CharacterManagement.Characters[characterToAttack].CurrentPosition;
        float totalChance = 0;
        List<BossPartType> attackingProspects = new List<BossPartType>();

        foreach (BossPart bossPart in BattleManager.Instance.CharacterManagement.Boss.Parts.Values)
        {
            if (BossUtils.PartCanAttackPosition(bossPart.PartType, characterPosition))
            {
                attackingProspects.Add(bossPart.PartType);
                totalChance += probabilities[bossPart.PartType];
            }
        }

        float attackRandomIndex = UnityEngine.Random.Range(0, totalChance);
        float randomnessCounter = 0;

        foreach (BossPartType prospect in attackingProspects)
        {
            BossPart bossPart = BattleManager.Instance.CharacterManagement.Boss.Parts[prospect];
            randomnessCounter += probabilities[prospect];
            if (randomnessCounter <= attackRandomIndex)
            {
                List<SetOfPositions> areasOfEffect = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[prospect].AreasOfEffect;

                foreach (SetOfPositions areaOfEffect in areasOfEffect)
                {
                    if (areaOfEffect.PositionInsideArea(characterPosition, bossPart.Position, bossPart.Orientation))
                    {
                        AddAttackActionToPile(bossPart, areaOfEffect);
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
