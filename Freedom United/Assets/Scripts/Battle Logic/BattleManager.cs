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
        Character character = characterManagement.Characters[characterID];
        battleGrid.CalculateRange(partyStats.Stats[characterID].AttackRange, character.CurrentPosition, false);
    }

    private void CalculateDefenseRange(CharacterID characterID)
    {
        Character character = characterManagement.Characters[characterID];
        battleGrid.CalculateRange(AttackRange.Short, character.CurrentPosition, true);
    }

    private void CalculateMoveRange(CharacterID characterID)
    {
        Character character = characterManagement.Characters[characterID];
        battleGrid.CalculateRange(character.CurrentPosition);
    }
}
