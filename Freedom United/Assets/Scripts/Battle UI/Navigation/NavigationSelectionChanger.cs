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
        if (navigationState.currentLevel == BattleSelectionLevel.Action)
        {
            navigationState.ActionSelection.Next();
            navigationState.currentAction.actionType = navigationState.ActionSelection.ActionSelected;
            navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
            BattleUIManager.Instance.ActionPileUI.RefreshView(0, 0);
        }
        if (navigationState.currentLevel == BattleSelectionLevel.Magic)
            navigationState.MagicSelection.Next();
        if (navigationState.currentLevel == BattleSelectionLevel.Cell)
            navigationState.CellSelection.Down();

        GameNotificationsManager.Instance.Notify(GameNotification.NavigationStateUpdated);
    }

    public void Up()
    {
        if (navigationState.currentLevel == BattleSelectionLevel.ActionPile)
            navigationState.ActionPileSelection.Previous();
        if (navigationState.currentLevel == BattleSelectionLevel.Action)
        {
            navigationState.ActionSelection.Previous();
            navigationState.currentAction.actionType = navigationState.ActionSelection.ActionSelected;
            navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
            BattleUIManager.Instance.ActionPileUI.RefreshView(0, 0);
        }
        if (navigationState.currentLevel == BattleSelectionLevel.Magic)
            navigationState.MagicSelection.Previous();
        if (navigationState.currentLevel == BattleSelectionLevel.Cell)
            navigationState.CellSelection.Up();

        GameNotificationsManager.Instance.Notify(GameNotification.NavigationStateUpdated);
    }

    public void Left()
    {
        if (navigationState.currentLevel == BattleSelectionLevel.Character)
            navigationState.CharacterSelection.Previous();
        else if (navigationState.currentLevel == BattleSelectionLevel.Cell)
            navigationState.CellSelection.Left();

        GameNotificationsManager.Instance.Notify(GameNotification.NavigationStateUpdated);
    }

    public void Right()
    {
        if (navigationState.currentLevel == BattleSelectionLevel.Character)
            navigationState.CharacterSelection.Next();
        else if (navigationState.currentLevel == BattleSelectionLevel.Cell)
            navigationState.CellSelection.Right();

        GameNotificationsManager.Instance.Notify(GameNotification.NavigationStateUpdated);
    }
}