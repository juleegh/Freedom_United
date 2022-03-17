using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManagement : MonoBehaviour, NotificationsListener
{
    [SerializeField] private List<Vector2Int> initialPositions;
    [SerializeField] private Vector2Int bossInitialPosition;
    [SerializeField] private BossConfig bossConfig;
    private Dictionary<CharacterID, Character> characters;
    public Dictionary<CharacterID, Character> Characters { get { return characters; } }
    private Boss boss;
    public Boss Boss { get { return boss; } }
    public BossConfig BossConfig { get { return bossConfig; } }

    public void ConfigureComponent()
    {
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.DependenciesLoaded, InitializeCharacters);
    }

    private void InitializeCharacters(GameNotificationData notificationData)
    {
        characters = new Dictionary<CharacterID, Character>();

        int index = 0;
        foreach (CharacterID stats in BattleManager.Instance.PartyStats.Stats.Keys)
        {
            characters.Add(stats, new Character(stats));
            SetCharacterInPosition(stats, initialPositions[index]);

            if (index < BattleManager.Instance.PartyStats.Stats.Count - 1)
                index++;
        }

        boss = new Boss(bossConfig);
    }

    public void SetCharacterInPosition(CharacterID characterID, Vector2Int newPosition)
    {
        characters[characterID].MoveToPosition(newPosition);
    }


    public BossPart GetBossPartInPosition(Vector2Int position)
    {
        foreach (BossPart bossPart in boss.Parts.Values)
        {
            if (bossPart.OccupiesPosition(position.x, position.y))
                return bossPart;
        }

        return null;
    }

    public Character GetCharacterInPosition(Vector2Int position)
    {
        foreach (Character character in characters.Values)
        {
            if (character.CurrentPosition == position)
                return character;
        }

        return null;
    }

    public void CheckForCharactersUnderBoss()
    {
        foreach (Character character in characters.Values)
        {
            Vector2Int characterPosition = character.CurrentPosition;
            if (GetBossPartInPosition(characterPosition) != null)
            {
                List<Vector2Int> availablePos = BattleGridUtils.GetAdjacentPositions(characterPosition);
                foreach (Vector2Int pos in availablePos)
                {
                    if (BattleManager.Instance.BattleGrid.ObstaclePositions.Contains(pos))
                        continue;

                    if (GetBossPartInPosition(pos) != null || GetCharacterInPosition(pos) != null)
                        continue;

                    character.MoveToPosition(pos);
                    BattleManager.Instance.BattleValues.CharacterTakeDamage(character.CharacterID, 15);

                    GameNotificationData notificationData = new GameNotificationData();
                    notificationData.Data[NotificationDataIDs.ActionOwner] = character.CharacterID;
                    notificationData.Data[NotificationDataIDs.CellPosition] = pos;
                    notificationData.Data[NotificationDataIDs.WasReckless] = true;
                    notificationData.Data[NotificationDataIDs.WasPushed] = true;
                    GameNotificationsManager.Instance.Notify(GameNotification.CharacterMoved, notificationData);

                    break;
                }
            }
        }
    }
}
