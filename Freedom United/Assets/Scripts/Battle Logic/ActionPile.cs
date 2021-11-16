using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActionPile : MonoBehaviour, NotificationsListener
{
    public List<ScheduledAction> actionsForTurn;
    public List<ScheduledAction> ActionsForTurn { get { return actionsForTurn; } }

    private List<CharacterID> charactersAvailable;
    public List<CharacterID> CharactersAvailable { get { return charactersAvailable; } }

    public void ConfigureComponent()
    {
        actionsForTurn = new List<ScheduledAction>();
        charactersAvailable = new List<CharacterID>();

        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleLoaded, Initialize);
    }

    private void Initialize()
    {
        foreach (CharacterID character in BattleManager.Instance.CharacterManagement.Characters.Keys.ToList())
            charactersAvailable.Add(character);
    }

    public void AddActionToPile(AllyAction action)
    {
        charactersAvailable.Remove(action.actionOwner);
        actionsForTurn.Add(action);
        actionsForTurn = actionsForTurn.OrderBy(o => (o.speed)).ToList();
        BattleUIManager.Instance.ActionPileUI.RefreshView(0, 0);
    }

    public void ClearList()
    {
        actionsForTurn.Clear();
        foreach (CharacterID character in BattleManager.Instance.CharacterManagement.Characters.Keys.ToList())
            charactersAvailable.Add(character);
    }
}
