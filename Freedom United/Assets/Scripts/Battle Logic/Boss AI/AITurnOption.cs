using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//[CreateAssetMenu(fileName = "AI Turn Option")]
[Serializable]
public class AITurnOption : AITurnAction
{
    [SerializeField] protected List<AICondition> conditions;
    [SerializeField] protected List<AITurnAction> attackActions;

    public bool CanExecute()
    {
        if (conditions.Count == 0)
            return true;

        Debug.LogWarning("-------- Evaluating: " + name + "--------");

        foreach (AICondition condition in conditions)
        {
            Debug.LogWarning("-------- Condition met: " + condition.GetType() + "? : " + condition.MeetsRequirement());
            if (!condition.MeetsRequirement())
                return false;
        }
        return true;
    }

    public override void Evaluate()
    {
        CheckAndAddToPile();
    }

    protected override bool CheckAndAddToPile()
    {
        if (CanExecute())
        {
            foreach (AITurnAction attackAction in attackActions)
            {
                attackAction.Evaluate();
                if (BattleManager.Instance.ActionPile.BossReachedLimit)
                {
                    return true;
                }
            }
            return true;
        }
        return false;
    }
}

