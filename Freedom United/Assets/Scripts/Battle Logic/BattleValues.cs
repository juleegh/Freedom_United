using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleValues : MonoBehaviour, NotificationsListener
{
    private Dictionary<BossPartType, float> bossHealthPoints;
    private Dictionary<CharacterID, float> partyHealthPoints;
    private Dictionary<CharacterID, float> partyWillPoints;
    private Dictionary<CharacterID, float> partyDefensePoints;
    private float currentBossHealth;
    private float totalBossHealth;
    private BossParts PartsList { get { return BattleManager.Instance.CharacterManagement.BossConfig.PartsList; } }

    public float BossHealth { get { return currentBossHealth; } }
    public float BaseBossHealth { get { return totalBossHealth; } }
    public Dictionary<BossPartType, float> BossPartsHealth { get { return bossHealthPoints; } }
    public Dictionary<CharacterID, float> PartyHealth { get { return partyHealthPoints; } }
    public Dictionary<CharacterID, float> PartyWill { get { return partyWillPoints; } }
    public Dictionary<CharacterID, float> PartyDefense { get { return partyDefensePoints; } }

    public void ConfigureComponent()
    {
        partyHealthPoints = new Dictionary<CharacterID, float>();
        partyWillPoints = new Dictionary<CharacterID, float>();
        partyDefensePoints = new Dictionary<CharacterID, float>();
        bossHealthPoints = new Dictionary<BossPartType, float>();
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleLoaded, InitializeValues);
    }

    public void CharacterTakeDamage(CharacterID character, float damageTaken)
    {
        partyHealthPoints[character] -= damageTaken;
        if (partyHealthPoints[character] <= 0)
        {
            partyHealthPoints[character] = BattleManager.Instance.PartyStats.Stats[character].BaseHealth;
            CharacterModifyWillPower(character, BattleManager.Instance.PartyStats.Stats[character].DeathWPDelta);
        }
        GameNotificationsManager.Instance.Notify(GameNotification.CharacterStatsChanged);
    }

    public void CharacterModifyWillPower(CharacterID character, int delta)
    {
        bool alive = IsAlive(character);

        partyWillPoints[character] += delta;
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

    public void CharacterModifyDefensePower(CharacterID character, float damage)
    {
        partyDefensePoints[character] -= damage;
        if (partyDefensePoints[character] <= 0)
        {
            partyDefensePoints[character] = 0;
        }

        GameNotificationsManager.Instance.Notify(GameNotification.CharacterStatsChanged);
    }

    public bool IsAlive(CharacterID character)
    {
        return partyWillPoints[character] > 0;
    }

    public bool CanDefend(CharacterID character)
    {
        return IsAlive(character) && partyDefensePoints[character] > 0;
    }

    public void BossTakeDamage(BossPartType partType, float damageTaken)
    {
        bool isDestroyed = BossPartIsDestroyed(partType);

        if (PartsList[partType].IsCore)
        {
            currentBossHealth -= damageTaken;
        }
        else if (bossHealthPoints[partType] > 0)
        {
            float acceptedDamage = bossHealthPoints[partType] < damageTaken ? bossHealthPoints[partType] : damageTaken;
            currentBossHealth -= acceptedDamage;
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
        return (PartsList[partType].IsCore && currentBossHealth <= 0) || bossHealthPoints[partType] <= 0;
    }

    private void InitializeValues(GameNotificationData notificationData)
    {
        currentBossHealth = 0;

        foreach (Character character in BattleManager.Instance.CharacterManagement.Characters.Values)
        {
            partyHealthPoints.Add(character.CharacterID, BattleManager.Instance.PartyStats.Stats[character.CharacterID].BaseHealth);
            partyWillPoints.Add(character.CharacterID, BattleManager.Instance.PartyStats.Stats[character.CharacterID].BaseWillPower);
            partyDefensePoints.Add(character.CharacterID, BattleManager.Instance.PartyStats.Stats[character.CharacterID].BaseShieldDurability);
        }

        foreach (BossPart bossPart in BattleManager.Instance
        .CharacterManagement.Boss.Parts.Values)
        {
            bossHealthPoints.Add(bossPart.PartType, PartsList[bossPart.PartType].BaseDurability);
            totalBossHealth += PartsList[bossPart.PartType].BaseDurability;
        }
        currentBossHealth = totalBossHealth;
    }


}
