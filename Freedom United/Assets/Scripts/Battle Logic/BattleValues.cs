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
    }

    public void CharacterModifyWillPower(CharacterID character, float percentage)
    {
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
    }

    public void BossTakeDamage(BossPartType partType, float damageTaken)
    {
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
    }

    public bool BossPartIsDestroyed(BossPartType partType)
    {
        return bossHealthPoints[partType] <= 0 || (PartsList[partType].IsCore && totalBossHealth <= 0);
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
