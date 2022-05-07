using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnAction : ScriptableObject
{
    public virtual void Evaluate()
    {
        bool result = CheckAndAddToPile();
        if (!result)
        {
            Debug.LogWarning("-------- Tried to: " + name + " but condition wasn't met");
        }
    }

    protected virtual bool CheckAndAddToPile()
    {
        return true;
    }

    protected void AddAttackActionToPile(BossPart bossPart, BossAttackInfo areaOfEffect)
    {
        BossAction currentAction = new BossAction();
        currentAction.actionOwner = bossPart.PartType;
        currentAction.deltaOfAction = areaOfEffect.attackShape;
        currentAction.position = areaOfEffect.pivotDelta;
        currentAction.actionType = BattleActionType.Attack;
        currentAction.speed = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[bossPart.PartType].AttackSpeed;
        BattleManager.Instance.ActionPile.AddActionToPile(currentAction);
        Debug.LogWarning("-- Attack Action Added: " + GetType());
    }

    protected void AddDefenseActionToPile(BossPart bossPart, SetOfPositions areaOfEffect)
    {
        BossAction currentAction = new BossAction();
        currentAction.actionOwner = bossPart.PartType;
        currentAction.deltaOfAction = areaOfEffect;
        currentAction.actionType = BattleActionType.Defend;
        currentAction.speed = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[bossPart.PartType].DefenseSpeed;
        BattleManager.Instance.ActionPile.AddActionToPile(currentAction);
        Debug.LogWarning("-- Defense Action Added: " + GetType());
    }

    protected void AddRotationActionToPile(BossPart bossPart, Vector2Int rotation)
    {
        BossAction currentAction = new BossAction();
        currentAction.actionOwner = bossPart.PartType;
        currentAction.position = rotation;
        currentAction.actionType = BattleActionType.Rotate;
        currentAction.speed = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[bossPart.PartType].RotateSpeed;
        BattleManager.Instance.ActionPile.AddActionToPile(currentAction);
        Debug.LogWarning("-- Rotation Action Added: " + GetType());
    }
}
