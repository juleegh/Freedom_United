using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationSelectionChanger
{
    private NavigationCurrentState navigationState { get { return BattleUINavigation.Instance.NavigationState; } }

    public void Down()
    {
        if (navigationState.currentLevel == BattleSelectionLevel.ActionPile)
            navigationState.ActionPileSelection.Next();
        else if (navigationState.currentLevel == BattleSelectionLevel.Cell)
            navigationState.CellSelection.Down();
        else
            return;

        GameAudio.Instance.AudioToEvent(AudioEvent.NavigationSide);
        GameNotificationsManager.Instance.Notify(GameNotification.NavigationStateUpdated);
    }

    public void Up()
    {
        if (navigationState.currentLevel == BattleSelectionLevel.ActionPile)
            navigationState.ActionPileSelection.Previous();
        else if (navigationState.currentLevel == BattleSelectionLevel.Cell)
            navigationState.CellSelection.Up();
        else
            return;

        GameAudio.Instance.AudioToEvent(AudioEvent.NavigationSide);
        GameNotificationsManager.Instance.Notify(GameNotification.NavigationStateUpdated);
    }

    public void Left()
    {
        if (navigationState.currentLevel == BattleSelectionLevel.Character)
            navigationState.CharacterSelection.Previous();
        else if (navigationState.currentLevel == BattleSelectionLevel.Action)
        {
            navigationState.ActionSelection.Previous();
            navigationState.currentAction.actionType = navigationState.ActionSelection.ActionSelected;
            navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
            navigationState.ActionPileSelection.JumpToPreview();
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Cell)
            navigationState.CellSelection.Left();
        else
            return;

        GameAudio.Instance.AudioToEvent(AudioEvent.NavigationSide);
        GameNotificationsManager.Instance.Notify(GameNotification.NavigationStateUpdated);
    }

    public void Right()
    {
        if (navigationState.currentLevel == BattleSelectionLevel.Character)
            navigationState.CharacterSelection.Next();
        else if (navigationState.currentLevel == BattleSelectionLevel.Action)
        {
            navigationState.ActionSelection.Next();
            navigationState.currentAction.actionType = navigationState.ActionSelection.ActionSelected;
            navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
            navigationState.ActionPileSelection.JumpToPreview();
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Cell)
            navigationState.CellSelection.Right();
        else
            return;

        GameAudio.Instance.AudioToEvent(AudioEvent.NavigationSide);
        GameNotificationsManager.Instance.Notify(GameNotification.NavigationStateUpdated);
    }
}