using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterStatsVisuals : StatsVisuals
{
    [SerializeField] private Image willBar;

    private CharacterID character;

    private Color willColor;

    public override void Initialize(string owner)
    {
        character = BattleGridUtils.GetCharacterID(owner);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.CharacterStatsChanged, RefreshBars);

        willColor = willBar.color;
        float willPower = BattleManager.Instance.BattleValues.PartyWill[character] / BattleManager.Instance.PartyStats.Stats[character].BaseWillPower;
        willBar.fillAmount = willPower;
    }

    protected override float GetCurrentHealth()
    {
        return BattleManager.Instance.BattleValues.PartyHealth[character];
    }

    protected override float GetBaseHealth()
    {
        return BattleManager.Instance.PartyStats.Stats[character].BaseHealth;
    }

    protected override void RefreshBars(GameNotificationData data)
    {
        base.RefreshBars(data);

        float willPower = BattleManager.Instance.BattleValues.PartyWill[character] / BattleManager.Instance.PartyStats.Stats[character].BaseWillPower;
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
