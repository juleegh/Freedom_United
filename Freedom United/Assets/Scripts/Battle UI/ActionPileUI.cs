using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPileUI : MonoBehaviour
{
    [SerializeField] private int actionsOnScreen;
    [SerializeField] private SimpleObjectPool previewsPool;
    [SerializeField] private Transform spellsContainer;
    [SerializeField] private GameObject UpIndicator;
    [SerializeField] private GameObject DownIndicator;
    private ScheduledActionPreview[] actionsPreviews;

    void Start()
    {
        LoadUI();
    }

    private void LoadUI()
    {
        actionsPreviews = new ScheduledActionPreview[actionsOnScreen];

        for (int i = 0; i < actionsOnScreen; i++)
        {
            actionsPreviews[i] = previewsPool.GetObject().GetComponent<ScheduledActionPreview>();
            actionsPreviews[i].transform.SetParent(spellsContainer);
            actionsPreviews[i].gameObject.SetActive(false);
        }
        RefreshView(0, 0);
    }

    public void RefreshView(int topAction, int selectedAction)
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
    }

    public void CleanView()
    {
        for (int i = 0; i < actionsOnScreen; i++)
        {
            actionsPreviews[i].gameObject.SetActive(false);
        }
    }

    public void RefreshSelectedPreview(int selectedTempo)
    {
        for (int i = 0; i < actionsOnScreen; i++)
        {
            actionsPreviews[i].ToggleSelected(i == selectedTempo);
        }
    }
}
