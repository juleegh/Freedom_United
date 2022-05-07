using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

//[CreateAssetMenu(fileName = "AI Attack Who Defends RPC")]
public class AIAttackWhoDefendsRPC : AITurnAction
{
    [Serializable]
    public class ChancesDictionary : SerializableDictionaryBase<BossPartType, float> { }

    [SerializeField] private ChancesDictionary probabilities;

    protected override bool CheckAndAddToPile()
    {
        return SelectDefender();
    }

    private bool PassCheck(BossPartType testPart)
    {
        return UnityEngine.Random.Range(0f, 1f) <= probabilities[testPart];
    }

    private bool SelectDefender()
    {
        List<BossPartConfig> parts = new List<BossPartConfig>(BattleManager.Instance.CharacterManagement.BossConfig.PartsList.Values);

        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.LastTurnRegisters)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionOwner) != TargetType.Character || BattleActionsUtils.GetTargetType(actionInfo.actionTarget) != TargetType.Character)
                continue;

            if (actionInfo.actionType != BattleActionType.Defend)
                continue;

            CharacterID defendedCharacter = BattleGridUtils.GetCharacterID(actionInfo.actionTarget);
            CharacterID counterPart = BattleManager.Instance.CharacterManagement.BossConfig.CharacterCounterPart;

            if (defendedCharacter != counterPart)
                continue;

            Vector2Int counterPartPosition = BattleManager.Instance.CharacterManagement.Characters[counterPart].CurrentPosition;

            foreach (KeyValuePair<BossPartType, float> part in probabilities)
            {
                if (!PassCheck(part.Key))
                {
                    continue;
                }

                if (BossUtils.PartCanAttackPosition(part.Key, counterPartPosition))
                {
                    BossPart bossPart = BattleManager.Instance.CharacterManagement.Boss.Parts[part.Key];
                    BossAttackInfo usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(bossPart, counterPartPosition);
                    AddAttackActionToPile(bossPart, usedAreaOfEffect);
                    return true;
                }
            }
        }

        return false;
    }
}
