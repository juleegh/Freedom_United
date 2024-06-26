using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationActionExecuter
{

    private NavigationCurrentState navigationState { get { return BattleUINavigation.Instance.NavigationState; } }

    public void Forward()
    {
        if (navigationState.currentLevel == BattleSelectionLevel.ActionPile)
        {
            if (!navigationState.ActionPileSelection.CurrentlyIsCharacter)
                return;

            navigationState.currentLevel = BattleSelectionLevel.Cancel;
            BattleUIManager.Instance.ToggleCancelPrompt(true);
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Character)
        {
            if (navigationState.CharacterSelection.FinishTurnSelected)
            {
                TurnExecutor.Instance.StartTurnExecution();
                return;
            }

            if (!BattleManager.Instance.BattleValues.IsAlive(navigationState.CharacterSelection.CharacterID))
                return;

            if (!BattleManager.Instance.ActionPile.CharacterAvailable(navigationState.CharacterSelection.CharacterID))
                return;

            navigationState.currentAction.actionOwner = navigationState.CharacterSelection.CharacterID;
            navigationState.currentLevel = BattleSelectionLevel.Action;
            navigationState.ActionSelection.Toggle(true);
            navigationState.currentAction.actionType = navigationState.ActionSelection.ActionSelected;
            navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
            navigationState.ActionPileSelection.JumpToPreview();
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Cancel)
        {
            if (!navigationState.ActionPileSelection.CurrentlyIsCharacter)
                return;

            BattleManager.Instance.ActionPile.RemoveActionFromPile(navigationState.ActionPileSelection.SelectedActionID);

            navigationState.currentLevel = BattleSelectionLevel.Character;
            navigationState.ActionPileSelection.Refresh();
            navigationState.CharacterSelection.Refresh();
            BattleUIManager.Instance.ToggleCancelPrompt(false);
            GameAudio.Instance.AudioToEvent(AudioEvent.RemoveAction);
            GameNotificationsManager.Instance.Notify(GameNotification.NavigationStateUpdated);
            return;
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Action)
        {
            if (navigationState.ActionSelection.ActionSelected == BattleActionType.Defend)
            {
                if (BattleManager.Instance.BattleValues.PartyDefense[navigationState.CharacterSelection.CharacterID] <= 0)
                    return;
            }

            BattleManager.Instance.CalculateActionRange(navigationState.ActionSelection.ActionSelected, navigationState.CharacterSelection.CharacterID);
            BattleGridUI.Instance.ToggleRange(BattleManager.Instance.BattleGrid.PositionsInRange, navigationState.ActionSelection.ActionSelected);

            navigationState.currentLevel = BattleSelectionLevel.Cell;
            Vector2Int position = BattleManager.Instance.ActionPile.GetTentativePosition(navigationState.CharacterSelection.CharacterID);
            navigationState.currentAction.position = position;
            navigationState.CellSelection.Toggle(true);
            navigationState.CellSelection.Initialize(position);
            GameAudio.Instance.AudioToEvent(AudioEvent.NavigationConfirm);
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Cell)
        {
            if (!BattleManager.Instance.BattleGrid.PositionsInRange.Contains(navigationState.CellSelection.SelectedPosition))
                return;

            if (navigationState.ActionSelection.ActionSelected == BattleActionType.Attack || navigationState.ActionSelection.ActionSelected == BattleActionType.Defend)
                navigationState.currentAction.position = navigationState.CellSelection.SelectedPosition - BattleManager.Instance.ActionPile.GetTentativePosition(navigationState.CharacterSelection.CharacterID);
            else
                navigationState.currentAction.position = navigationState.CellSelection.SelectedPosition;

            CreateAllyAction();
            navigationState.ResetActionSelection();
        }

        GameAudio.Instance.AudioToEvent(AudioEvent.NavigationConfirm);
        GameNotificationsManager.Instance.Notify(GameNotification.NavigationStateUpdated);
    }

    public void Backwards()
    {
        if (navigationState.currentLevel == BattleSelectionLevel.Cancel)
        {
            navigationState.currentLevel = BattleSelectionLevel.ActionPile;
            navigationState.CharacterSelection.Refresh();
            BattleUIManager.Instance.ToggleCancelPrompt(false);
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.ActionPile)
        {
            navigationState.currentLevel = BattleSelectionLevel.Character;
            navigationState.ActionPileSelection.Refresh();
            navigationState.CharacterSelection.Refresh();
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Character)
        {
            navigationState.currentLevel = BattleSelectionLevel.ActionPile;
            navigationState.CharacterSelection.Refresh();
            navigationState.ActionPileSelection.Refresh();
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Cancel)
        {
            navigationState.currentLevel = BattleSelectionLevel.Character;
            navigationState.ActionPileSelection.Refresh();
            navigationState.CharacterSelection.Refresh();
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Action)
        {
            navigationState.currentLevel = BattleSelectionLevel.Character;
            navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
            navigationState.ActionPileSelection.Refresh();
            navigationState.ActionSelection.Toggle(false);
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Cell)
        {
            BattleGridUI.Instance.ToggleRange();
            navigationState.currentLevel = BattleSelectionLevel.Action;
            navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
            navigationState.ActionPileSelection.Refresh();
            navigationState.MagicSelection.Toggle(false);
            navigationState.CellSelection.Toggle(false);
        }
        navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
        GameAudio.Instance.AudioToEvent(AudioEvent.NavigationBack);
        GameNotificationsManager.Instance.Notify(GameNotification.NavigationStateUpdated);
    }

    private void CreateAllyAction()
    {
        BattleManager.Instance.ActionPile.AddActionToPile(navigationState.currentAction);
        BattleGridUI.Instance.ToggleRange();
    }
}