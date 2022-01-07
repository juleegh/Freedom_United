using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterStatsVisuals : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image willBar;

    private CharacterID character;
    private float barDelay = 0.3f;

    private Color healthColor;
    private Color willColor;

    public void Initialize(CharacterID characterID)
    {
        character = characterID;
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.CharacterStatsChanged, RefreshBars);

        healthColor = healthBar.color;
        willColor = willBar.color;

        float health = BattleManager.Instance.BattleValues.PartyHealth[character] / BattleManager.Instance.PartyStats.Stats[character].BaseHealth;
        float willPower = BattleManager.Instance.BattleValues.PartyWill[character] / BattleManager.Instance.PartyStats.Stats[character].BaseWillPower;

        healthBar.fillAmount = health;
        willBar.fillAmount = willPower;
    }

    private void RefreshBars(GameNotificationData data)
    {
        float health = BattleManager.Instance.BattleValues.PartyHealth[character] / BattleManager.Instance.PartyStats.Stats[character].BaseHealth;
        float willPower = BattleManager.Instance.BattleValues.PartyWill[character] / BattleManager.Instance.PartyStats.Stats[character].BaseWillPower;

        if (!Mathf.Approximately(health, healthBar.fillAmount))
        {
            Color origin = health < healthBar.fillAmount ? Color.red : Color.white;
            healthBar.color = origin;
            healthBar.DOColor(healthColor, barDelay);
            healthBar.DOFillAmount(health, barDelay);
        }
        else
            healthBar.fillAmount = health;

        if (!Mathf.Approximately(willPower, willBar.fillAmount))
        {
            Color origin = willPower < willBar.fillAmount ? Color.red : Color.white;
            willBar.color = origin;
            willBar.DOColor(willColor, barDelay);
            willBar.DOFillAmount(willPower, barDelay);
        }
        else
            willBar.fillAmount = willPower;
    }
}
