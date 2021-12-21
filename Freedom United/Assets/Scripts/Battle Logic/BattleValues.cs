using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleValues : MonoBehaviour, NotificationsListener
{
    private Dictionary<BossPartType, float> bossHealthPoints;
    private Dictionary<CharacterID, float> partyHealthPoints;
    private float totalBossHealth;
    private BossParts PartsList { get { return BattleManager.Instance.CharacterManagement.BossConfig.PartsList; } }

    public float BossHealth { get { return totalBossHealth; } }
    public Dictionary<BossPartType, float> BossPartsHealth { get { return bossHealthPoints; } }
    public Dictionary<CharacterID, float> PartyHealth { get { return partyHealthPoints; } }

    public void ConfigureComponent()
    {
        partyHealthPoints = new Dictionary<CharacterID, float>();
        bossHealthPoints = new Dictionary<BossPartType, float>();
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleLoaded, InitializeHealth);
    }

    public void CharacterTakeDamage(CharacterID character, float damageTaken)
    {
        partyHealthPoints[character] -= damageTaken;
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

    private void InitializeHealth(GameNotificationData notificationData)
    {
        totalBossHealth = BattleManager.Instance.CharacterManagement.BossConfig.BaseHealth;

        foreach (Character character in BattleManager.Instance.CharacterManagement.Characters.Values)
        {
            partyHealthPoints.Add(character.CharacterID, BattleManager.Instance.PartyStats.Stats[character.CharacterID].BaseHealth);
        }

        foreach (BossPart bossPart in BattleManager.Instance
        .CharacterManagement.Boss.Parts)
        {
            bossHealthPoints.Add(bossPart.PartType, PartsList[bossPart.PartType].BaseDurability);
        }
    }


}
