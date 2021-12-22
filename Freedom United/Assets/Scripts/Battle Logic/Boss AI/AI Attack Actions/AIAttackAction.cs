using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackAction : ScriptableObject
{
    public void AddToPile()
    {
        AddToActionPile();
    }

    protected virtual void AddToActionPile()
    {
        AddToPileDefault();
    }

    protected void AddToPileDefault()
    {
        // Chose an attack targeting the wrath character. If this cannot be performed, choose any attack at random from the list of available attacks and do it
        bool result = SelectCounterPartTarget();
        if (!result)
            SelectRandomTarget();
    }

    protected bool SelectCounterPartTarget()
    {
        CharacterID counterPart = BattleManager.Instance.CharacterManagement.BossConfig.CharacterCounterPart;
        Vector2Int counterPartPosition = BattleManager.Instance.CharacterManagement.Characters[counterPart].CurrentPosition;

        BossPart attackingPart = BossUtils.GetPartWhoCanAttackPosition(counterPartPosition);
        AreaOfEffect usedAreaOfEffect = null;

        if (attackingPart != null)
        {
            usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(attackingPart, counterPartPosition);
            AddAttackActionToPile(attackingPart, usedAreaOfEffect);
            return true;
        }

        return false;
    }

    protected bool SelectRandomTarget()
    {
        List<Vector2Int> playerPositions = new List<Vector2Int>();
        foreach (Character character in BattleManager.Instance.CharacterManagement.Characters.Values)
        {
            if (BattleManager.Instance.BattleValues.PartyHealth[character.CharacterID] <= 0)
                continue;

            playerPositions.Add(character.CurrentPosition);
        }

        bool selected = false;
        while (!selected && playerPositions.Count > 0)
        {
            int randomPosition = Random.Range(0, playerPositions.Count);
            Vector2Int position = playerPositions[randomPosition];
            BossPart attackingPart = BossUtils.GetPartWhoCanAttackPosition(position);
            playerPositions.Remove(position);

            if (attackingPart != null)
            {
                AddAttackActionToPile(attackingPart, BossUtils.GetAreaOfEffectForPosition(attackingPart, position));
                return true;
            }
        }

        return false;
    }

    protected void AddAttackActionToPile(BossPart bossPart, AreaOfEffect areaOfEffect)
    {
        BossAction currentAction = new BossAction();
        currentAction.actionOwner = bossPart.PartType;
        currentAction.areaOfEffect = areaOfEffect;
        currentAction.actionType = BattleActionType.Attack;
        currentAction.speed = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[bossPart.PartType].AttackSpeed;
        BattleManager.Instance.ActionPile.AddActionToPile(currentAction);
    }

    protected void AddDefenseActionToPile(BossPart bossPart, AreaOfEffect areaOfEffect)
    {
        BossAction currentAction = new BossAction();
        currentAction.actionOwner = bossPart.PartType;
        currentAction.areaOfEffect = areaOfEffect;
        currentAction.actionType = BattleActionType.Defend;
        currentAction.speed = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[bossPart.PartType].DefenseSpeed;
        BattleManager.Instance.ActionPile.AddActionToPile(currentAction);
    }
}
