using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AITurnOption
{
    [SerializeField] private AICondition condition;
    [SerializeField] private List<AIAttackAction> attackActions;

    public bool CanExecute()
    {
        if (condition == null)
            return false;

        return condition.MeetsRequirement();
    }

    public void ExecuteActions()
    {
        foreach (AIAttackAction attackAction in attackActions)
        {
            attackAction.AddToPile();
        }
    }
}
