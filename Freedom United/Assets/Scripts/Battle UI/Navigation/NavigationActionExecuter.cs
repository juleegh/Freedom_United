using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationActionExecuter
{

    private NavigationCurrentState navigationState { get { return BattleUINavigation.Instance.NavigationState; } }

    public void Forward()
    {
        if (navigationState.currentLevel == BattleSelectionLevel.Character)
        {
            if (navigationState.CharacterSelection.FinishTurnSelected)
            {
                TurnExecutor.Instance.StartTurnExecution();
                return;
            }

            if (!BattleManager.Instance.BattleValues.IsAlive(navigationState.CharacterSelection.CharacterID))
                return;

            if (!BattleManager.Instance.ActionPile.CharacterAvailable(navigationState.CharacterSelection.CharacterID))
            {
                navigationState.currentLevel = BattleSelectionLevel.Cancel;
                BattleUIManager.Instance.CancelUI.ToggleVisible(true);
                navigationState.ActionPileSelection.SetAsSelected(navigationState.CharacterSelection.CharacterID);
            }
            else
            {
                navigationState.currentAction.actionOwner = navigationState.CharacterSelection.CharacterID;
                navigationState.currentLevel = BattleSelectionLevel.Action;
                navigationState.ActionSelection.Toggle(true);
                navigationState.currentAction.actionType = navigationState.ActionSelection.ActionSelected;
                navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
                navigationState.ActionPileSelection.Refresh();
            }
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Cancel)
        {
            BattleUIManager.Instance.CancelUI.ToggleVisible(false);
            BattleManager.Instance.ActionPile.RemoveActionFromPile(navigationState.CharacterSelection.CharacterID);

            navigationState.currentLevel = BattleSelectionLevel.Character;
            navigationState.ActionPileSelection.Refresh();
            navigationState.CharacterSelection.Refresh();
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Action)
        {
            if (navigationState.ActionSelection.ActionSelected == BattleActionType.Magic)
            {
                navigationState.currentLevel = BattleSelectionLevel.Magic;
                navigationState.MagicSelection.Toggle(true);
            }
            else
            {
                BattleManager.Instance.CalculateActionRange(navigationState.ActionSelection.ActionSelected, navigationState.CharacterSelection.CharacterID);
                BattleGridUI.Instance.ToggleRange(BattleManager.Instance.BattleGrid.PositionsInRange, navigationState.ActionSelection.ActionSelected);

                navigationState.currentLevel = BattleSelectionLevel.Cell;
                Vector2Int position = BattleManager.Instance.BattleGrid.PositionsInRange[0];
                if (navigationState.ActionSelection.ActionSelected == BattleActionType.MoveSafely || navigationState.ActionSelection.ActionSelected == BattleActionType.MoveFast)
                    position = BattleManager.Instance.CharacterManagement.Characters[navigationState.CharacterSelection.CharacterID].CurrentPosition;
                navigationState.currentAction.position = position;
                navigationState.CellSelection.Toggle(true);
                navigationState.CellSelection.Initialize(position);
            }
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Magic)
        {
            //navigationState.currentLevel = BattleSelectionLevel.Cell;
            //navigationState.CellSelection.Toggle(true);
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Cell)
        {
            if (!BattleManager.Instance.BattleGrid.PositionsInRange.Contains(navigationState.CellSelection.SelectedPosition))
                return;

            if (navigationState.ActionSelection.ActionSelected == BattleActionType.Attack || navigationState.ActionSelection.ActionSelected == BattleActionType.Defend)
                navigationState.currentAction.position = navigationState.CellSelection.SelectedPosition - BattleManager.Instance.CharacterManagement.Characters[navigationState.CharacterSelection.CharacterID].CurrentPosition;
            else
                navigationState.currentAction.position = navigationState.CellSelection.SelectedPosition;

            CreateAllyAction();
            navigationState.ResetActionSelection();
        }

        GameNotificationsManager.Instance.Notify(GameNotification.NavigationStateUpdated);
    }

    public void Backwards()
    {
        if (navigationState.currentLevel == BattleSelectionLevel.ActionPile)
        {
            navigationState.currentLevel = BattleSelectionLevel.Character;
            navigationState.ActionPileSelection.Refresh();
            navigationState.CharacterSelection.Refresh();
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Character)
        {
            navigationState.currentLevel = BattleSelectionLevel.ActionPile;
            navigationState.ActionPileSelection.Refresh();
            navigationState.CharacterSelection.Refresh();
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Cancel)
        {
            BattleUIManager.Instance.CancelUI.ToggleVisible(false);
            navigationState.currentLevel = BattleSelectionLevel.Character;
            navigationState.ActionPileSelection.Refresh();
            navigationState.CharacterSelection.Refresh();
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Action)
        {
            navigationState.currentLevel = BattleSelectionLevel.Character;
            navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
            BattleUIManager.Instance.ActionPileUI.RefreshView(0, 0);
            navigationState.ActionSelection.Toggle(false);
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Magic)
        {
            navigationState.currentLevel = BattleSelectionLevel.Action;
            navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
            BattleUIManager.Instance.ActionPileUI.RefreshView(0, 0);
            navigationState.MagicSelection.Toggle(false);
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Cell)
        {
            BattleGridUI.Instance.ToggleRange();
            navigationState.currentLevel = BattleSelectionLevel.Action;
            navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
            BattleUIManager.Instance.ActionPileUI.RefreshView(0, 0);
            navigationState.MagicSelection.Toggle(false);
            navigationState.CellSelection.Toggle(false);
        }
        navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
        GameNotificationsManager.Instance.Notify(GameNotification.NavigationStateUpdated);
    }

    private void CreateAllyAction()
    {
        BattleManager.Instance.ActionPile.AddActionToPile(navigationState.currentAction);
        BattleGridUI.Instance.ToggleRange();
    }
}