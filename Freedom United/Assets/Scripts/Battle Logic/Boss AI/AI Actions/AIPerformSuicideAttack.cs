using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Perform Suicide Attack")]
public class AIPerformSuicideAttack : AITurnAction
{
    protected override bool CheckAndAddToPile()
    {
        return PerformSuicideAttack();
    }

    private bool PerformSuicideAttack()
    {
        BossAction currentAction = new BossAction();
        currentAction.actionOwner = BattleManager.Instance.CharacterManagement.Boss.Core.PartType;
        currentAction.actionType = BattleActionType.SuicideAttack;
        currentAction.speed = BattleManager.Instance.CharacterManagement.BossConfig.SuicideAttackSpeed;
        BattleManager.Instance.ActionPile.AddActionToPile(currentAction);
        Debug.LogWarning("-- Suicide Attack Action Added: " + GetType());

        return true;
    }
}
