using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPileUI : MonoBehaviour, NotificationsListener
{
    [SerializeField] private int actionsOnScreen;
    [SerializeField] private SimpleObjectPool previewsPool;
    [SerializeField] private Transform spellsContainer;
    [SerializeField] private GameObject UpIndicator;
    [SerializeField] private GameObject DownIndicator;
    private ScheduledActionPreview[] actionsPreviews;
    public int ActionsOnScreen { get { return actionsOnScreen; } }
    private bool focus { get { return BattleUINavigation.Instance.CurrentLevel == BattleSelectionLevel.ActionPile || BattleUINavigation.Instance.CurrentLevel == BattleSelectionLevel.Cancel; } }

    public void ConfigureComponent()
    {
        GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleUILoaded, LoadUI);
    }

    private void LoadUI(GameNotificationData notificationData)
    {
        actionsPreviews = new ScheduledActionPreview[actionsOnScreen];

        for (int i = 0; i < actionsOnScreen; i++)
        {
            actionsPreviews[i] = previewsPool.GetObject().GetComponent<ScheduledActionPreview>();
            actionsPreviews[i].transform.SetParent(spellsContainer);
            actionsPreviews[i].gameObject.SetActive(false);
        }
        RefreshView();
    }

    public void RefreshView(int topAction = 0, int selectedAction = 0)
    {
        int previewIndex = 0;

        List<ScheduledAction> scheduledActions = BattleManager.Instance.ActionPile.ActionsForTurn;

        for (int i = topAction; i < topAction + actionsOnScreen; i++)
        {
            if (i >= scheduledActions.Count)
            {
                actionsPreviews[previewIndex].gameObject.SetActive(false);
                previewIndex++;
                continue;
            }

            actionsPreviews[previewIndex].gameObject.SetActive(true);
            actionsPreviews[previewIndex].ConfigVisuals(scheduledActions[i]);
            previewIndex++;
        }

        UpIndicator.SetActive(topAction > 0);
        DownIndicator.SetActive(topAction + actionsOnScreen <= scheduledActions.Count - 1);
        RefreshSelectedPreview(selectedAction);
    }

    public void CleanView()
    {
        for (int i = 0; i < actionsOnScreen; i++)
        {
            actionsPreviews[i].gameObject.SetActive(false);
        }
    }

    public void RefreshSelectedPreview(int selectedAction)
    {
        for (int i = 0; i < actionsOnScreen; i++)
        {
            UIStatus actionStatus = UIStatus.Regular;
            if (i == selectedAction && focus)
            {
                actionStatus = UIStatus.Highlighted;
                if (BattleUINavigation.Instance.CurrentLevel == BattleSelectionLevel.Cancel)
                    actionStatus = UIStatus.Canceling;
            }
            actionsPreviews[i].UpdateStatus(actionStatus);
        }
    }

    public void UpdateStatus(int selectedAction, UIStatus status)
    {
        actionsPreviews[selectedAction].UpdateStatus(status);
    }

}
