using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActionPile : MonoBehaviour
{
    private List<ScheduledAction> actionsForTurn;
    public List<ScheduledAction> ActionsForTurn { get { return actionsForTurn; } }

    void Awake()
    {
        actionsForTurn = new List<ScheduledAction>();
    }

    public void AddActionToPile(ScheduledAction nextAction)
    {
        actionsForTurn = new List<ScheduledAction>();
        actionsForTurn = actionsForTurn.OrderBy(o => (o.Speed)).ToList();
    }

    public void ClearList()
    {
        actionsForTurn.Clear();
    }
}
