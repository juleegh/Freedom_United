using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//[CreateAssetMenu(fileName = "AI Turn Option")]
[Serializable]
public class AITurnOption : ScriptableObject
{
    [SerializeField] protected AICondition condition;
    [SerializeField] protected List<AITurnAction> attackActions;

    public bool CanExecute()
    {
        if (condition == null)
            return true;

        return condition.MeetsRequirement();
    }

    public void SelectActions()
    {
        Debug.LogError("-------- Condition met: " + condition.GetType() + "--------");
        foreach (AITurnAction attackAction in attackActions)
        {
            attackAction.AddToPile();
            if (BattleManager.Instance.ActionPile.BossReachedLimit)
            {
                return;
            }
        }
    }
}

