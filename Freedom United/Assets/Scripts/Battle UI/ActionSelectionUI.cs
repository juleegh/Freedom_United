using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionUI : MonoBehaviour
{
    [SerializeField] private ActionSelectionOption[] actionPreviews;
    [SerializeField] private GameObject sectionContainer;

    void Start()
    {
        Initialize();
        //ToggleVisible(false);
    }

    private void Initialize()
    {
        for (int i = 0; i < actionPreviews.Length; i++)
        {
            actionPreviews[i].Config(BattleActionsUtils.GetActionsList()[i]);
        }
    }

    public void ToggleVisible(bool visible)
    {
        sectionContainer.SetActive(visible);
    }

    public void RefreshSelectedAction(int selectedCharacter)
    {
        for (int i = 0; i < actionPreviews.Length; i++)
        {
            actionPreviews[i].ToggleSelected(i == selectedCharacter);
        }
    }
}
