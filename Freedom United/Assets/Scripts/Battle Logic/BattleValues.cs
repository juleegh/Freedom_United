using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleValues : MonoBehaviour, NotificationsListener
{
    private Dictionary<BossPartType, float> bossHealthPoints;
    private Dictionary<CharacterID, float> partyHealthPoints;
    private Dictionary<CharacterID, float> partyWillPoints;
    private float totalBossHealth;
    private BossParts PartsList { get { return BattleManager.Instance.CharacterManagement.BossConfig.PartsList; } }

    public float BossHealth { get { return totalBossHealth; } }
    public Dictionary<BossPartType, float> BossPartsHealth { get { return bossHealthPoints; } }
    public Dictionary<CharacterID, float> PartyHealth { get { return partyHealthPoints; } }
    public Dictionary<CharacterID, float> PartyWill { get { return partyWillPoints; } }

    public void ConfigureComponent()
    {
        partyHealthPoints = new Dictionary<CharacterID, float>();
        partyWillPoints = new Dictionary<CharacterID, float>();
        bossHealthPoints = new Dictionary<BossPartType, float>();
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleLoaded, InitializeValues);
    }

    public void CharacterTakeDamage(CharacterID character, float damageTaken)
    {
        partyHealthPoints[character] -= damageTaken;
        if (partyHealthPoints[character] <= 0)
        {
            partyHealthPoints[character] = BattleManager.Instance.PartyStats.Stats[character].BaseHealth;
            CharacterModifyWillPower(character, BattleGridUtils.DeathWillPercentage);
        }
        GameNotificationsManager.Instance.Notify(GameNotification.CharacterStatsChanged);
    }

    public void CharacterModifyWillPower(CharacterID character, float percentage)
    {
        bool alive = IsAlive(character);

        partyWillPoints[character] += BattleManager.Instance.PartyStats.Stats[character].BaseWillPower * percentage;
        if (partyWillPoints[character] <= 0)
        {
            partyWillPoints[character] = 0;
            partyHealthPoints[character] = 0;
        }
        else if (partyWillPoints[character] > BattleManager.Instance.PartyStats.Stats[character].BaseWillPower)
        {
            partyWillPoints[character] = BattleManager.Instance.PartyStats.Stats[character].BaseWillPower;
        }

        GameNotificationsManager.Instance.Notify(GameNotification.CharacterStatsChanged);
        if (alive != IsAlive(character))
        {
            GameNotificationData notificationData = new GameNotificationData();
            notificationData.Data[NotificationDataIDs.Deceased] = character.ToString();
            GameNotificationsManager.Instance.Notify(GameNotification.RecentDeath, notificationData);
        }
    }

    public bool IsAlive(CharacterID character)
    {
        return partyWillPoints[character] > 0;
    }

    public void BossTakeDamage(BossPartType partType, float damageTaken)
    {
        bool isDestroyed = BossPartIsDestroyed(partType);

        if (PartsList[partType].IsCore)
        {
            totalBossHealth -= damageTaken;
        }
        else if (bossHealthPoints[partType] > 0)
        {
            float acceptedDamage = bossHealthPoints[partType] < damageTaken ? bossHealthPoints[partType] : damageTaken;
            totalBossHealth -= acceptedDamage;
            bossHealthPoints[partType] -= acceptedDamage;
        }

        if (isDestroyed != BossPartIsDestroyed(partType))
        {
            GameNotificationData notificationData = new GameNotificationData();
            notificationData.Data[NotificationDataIDs.Deceased] = partType.ToString();
            GameNotificationsManager.Instance.Notify(GameNotification.RecentDeath, notificationData);
        }
    }

    public bool BossPartIsDestroyed(BossPartType partType)
    {
        return (PartsList[partType].IsCore && totalBossHealth <= 0) || bossHealthPoints[partType] <= 0;
    }

    private void InitializeValues(GameNotificationData notificationData)
    {
        totalBossHealth = 0;

        foreach (Character character in BattleManager.Instance.CharacterManagement.Characters.Values)
        {
            partyHealthPoints.Add(character.CharacterID, BattleManager.Instance.PartyStats.Stats[character.CharacterID].BaseHealth);
            partyWillPoints.Add(character.CharacterID, BattleManager.Instance.PartyStats.Stats[character.CharacterID].BaseWillPower);
        }

        foreach (BossPart bossPart in BattleManager.Instance
        .CharacterManagement.Boss.Parts.Values)
        {
            bossHealthPoints.Add(bossPart.PartType, PartsList[bossPart.PartType].BaseDurability);
            totalBossHealth += PartsList[bossPart.PartType].BaseDurability;
        }
    }


}
