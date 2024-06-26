using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour, NotificationsListener
{
    private static BattleManager instance;
    public static BattleManager Instance { get { return instance; } }

    private BattleGrid battleGrid;
    public BattleGrid BattleGrid { get { return battleGrid; } }

    private CharacterManagement characterManagement;
    public CharacterManagement CharacterManagement { get { return characterManagement; } }

    private ActionPile actionPile;
    public ActionPile ActionPile { get { return actionPile; } }

    private MagicManagement magicManagement;
    public MagicManagement MagicManagement { get { return magicManagement; } }

    private BattleValues battleValues;
    public BattleValues BattleValues { get { return battleValues; } }

    [SerializeField] private PartyStats partyStats;
    public PartyStats PartyStats { get { return partyStats; } }

    public void ConfigureComponent()
    {
        instance = this;
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.DependenciesLoaded, Initialize);
    }

    private void Initialize(GameNotificationData notificationData)
    {
        battleGrid = GetComponent<BattleGrid>();
        characterManagement = GetComponent<CharacterManagement>();
        actionPile = GetComponent<ActionPile>();
        magicManagement = GetComponent<MagicManagement>();
        battleValues = GetComponent<BattleValues>();
        magicManagement.Initialize();
        GameNotificationsManager.Instance.Notify(GameNotification.BattleLoaded);
    }

    public void CalculateActionRange(BattleActionType actionType, CharacterID characterID)
    {
        switch (actionType)
        {
            case BattleActionType.Attack:
                CalculateAttackRange(characterID);
                break;
            case BattleActionType.Defend:
                CalculateDefenseRange(characterID);
                break;
            case BattleActionType.MoveFast:
            case BattleActionType.MoveSafely:
                CalculateMoveRange(characterID);
                break;
        }
    }

    private void CalculateAttackRange(CharacterID characterID)
    {
        Vector2Int currentPosition = BattleManager.Instance.ActionPile.GetTentativePosition(characterID);
        battleGrid.CalculateRange(partyStats.Stats[characterID].AttackRange, currentPosition, false);
    }

    private void CalculateDefenseRange(CharacterID characterID)
    {
        Vector2Int currentPosition = BattleManager.Instance.ActionPile.GetTentativePosition(characterID);
        battleGrid.CalculateRange(AttackRange.Short, currentPosition, true);
    }

    private void CalculateMoveRange(CharacterID characterID)
    {
        Vector2Int currentPosition = BattleManager.Instance.ActionPile.GetTentativePosition(characterID);
        battleGrid.CalculateRange(currentPosition);
    }
}
