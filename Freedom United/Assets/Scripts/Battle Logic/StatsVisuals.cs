using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StatsVisuals : MonoBehaviour
{
    [SerializeField] protected Image healthBar;

    protected float barDelay = 0.3f;
    private BossPartType partType;

    protected Color healthColor;

    public virtual void Initialize(string owner)
    {
        partType = BattleGridUtils.GetBossPart(owner);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BossStatsModified, RefreshBars);
        InitializeHealthBar();
    }

    protected void InitializeHealthBar()
    {
        healthColor = healthBar.color;
        float health = 1;
        healthBar.fillAmount = health;
    }

    protected virtual float GetCurrentHealth()
    {
        return (float) BattleManager.Instance.BattleValues.BossPartsHealth[partType];
    }

    protected virtual float GetBaseHealth()
    {
        return (float) BattleManager.Instance.CharacterManagement.BossConfig.PartsList[partType].BaseDurability;
    }

    protected virtual void RefreshBars(GameNotificationData data)
    {
        float health = (float) GetCurrentHealth() / GetBaseHealth();

        if (!Mathf.Approximately(health, healthBar.fillAmount))
        {
            Color origin = health < healthBar.fillAmount ? Color.red : Color.white;
            healthBar.color = origin;
            healthBar.DOColor(healthColor, barDelay);
            healthBar.DOFillAmount(health, barDelay);
        }
        else
            healthBar.fillAmount = health;
    }
}
