using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossInfoUI : MonoBehaviour, NotificationsListener
{
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private Image healthFill;

    public void ConfigureComponent()
    {
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BossStatsModified, UpdateBar);
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleLoaded, Initialize);
    }

    private void Initialize()
    {
        healthFill.fillAmount = 1f;
        bossName.text = BattleManager.Instance.CharacterManagement.BossConfig.BossName;
    }

    private void UpdateBar()
    {
        float current = BattleManager.Instance.BattleValues.BossHealth;
        float max = BattleManager.Instance.CharacterManagement.BossConfig.BaseHealth;

        healthFill.fillAmount = current / max;
    }
}
