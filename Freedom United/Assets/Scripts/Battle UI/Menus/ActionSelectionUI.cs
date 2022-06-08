using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using TMPro;

public class ActionSelectionUI : MonoBehaviour, NotificationsListener
{
    [Serializable]
    public class ActionIcons : SerializableDictionaryBase<BattleActionType, Sprite> { }

    [SerializeField] private ActionIcons actionIcons;

    [SerializeField] private ActionSelectionOption previousAction;
    [SerializeField] private ActionSelectionOption currentAction;
    [SerializeField] private ActionSelectionOption nextAction;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private GameObject sectionContainer;

    public void ConfigureComponent()
    {
        //GameNotificationsManager.Instance.AddActionToEvent(GameNotification.BattleLoaded, Initialize);
    }

    public void ToggleVisible(bool visible)
    {
        sectionContainer.SetActive(visible);
    }

    public void SetHorizontalPosition(Vector3 position)
    {
        Vector3 currentPosition = GetComponent<RectTransform>().transform.position;
        currentPosition.x = position.x;
        GetComponent<RectTransform>().transform.position = currentPosition;
    }

    public void RefreshSelectedAction(BattleActionType previous, BattleActionType current, BattleActionType next)
    {
        previousAction.Config(actionIcons[previous]);
        currentAction.Config(actionIcons[current]);
        nextAction.Config(actionIcons[next]);
        actionText.text = current.ToString();
    }
}
