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
            if (!BattleManager.Instance.ActionPile.CharactersAvailable.Contains(navigationState.CharacterSelection.CharacterID))
                return;

            navigationState.currentAction.actionOwner = navigationState.CharacterSelection.CharacterID;
            navigationState.currentLevel = BattleSelectionLevel.Action;
            navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
            BattleUIManager.Instance.ActionPileUI.RefreshView(0, 0);
            navigationState.ActionSelection.Toggle(true);
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
                if (navigationState.ActionSelection.ActionSelected == BattleActionType.Attack)
                {
                    BattleManager.Instance.CalculateAttackRange(navigationState.CharacterSelection.CharacterID);
                    BattleGridUI.Instance.ToggleRange(true);
                }
                else if (navigationState.ActionSelection.ActionSelected == BattleActionType.Defend)
                {
                    BattleManager.Instance.CalculateDefenseRange(navigationState.CharacterSelection.CharacterID);
                    BattleGridUI.Instance.ToggleRange(true);
                }
                else if (navigationState.ActionSelection.ActionSelected == BattleActionType.MoveSafely || navigationState.ActionSelection.ActionSelected == BattleActionType.MoveFast)
                {
                    BattleManager.Instance.CalculateMoveRange(navigationState.CharacterSelection.CharacterID);
                    BattleGridUI.Instance.ToggleRange(true);
                }

                navigationState.currentLevel = BattleSelectionLevel.Cell;
                Vector2Int position = navigationState.CharacterSelection.SelectedCharacter.CurrentPosition;
                navigationState.CellSelection.Toggle(true);
                navigationState.CellSelection.Initialize(BattleManager.Instance.BattleGrid.PositionsInRange[0]);
            }
        }
        else if (navigationState.currentLevel == BattleSelectionLevel.Magic)
        {
            navigationState.currentLevel = BattleSelectionLevel.Cell;
            navigationState.CellSelection.Toggle(true);
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
            BattleGridUI.Instance.ToggleRange(false);
            navigationState.currentLevel = BattleSelectionLevel.Action;
            navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
            BattleUIManager.Instance.ActionPileUI.RefreshView(0, 0);
            navigationState.MagicSelection.Toggle(false);
            navigationState.CellSelection.Toggle(false);
        }
        navigationState.currentAction.speed = BattleActionsUtils.GetActionSpeed();
    }

    private void CreateAllyAction()
    {
        BattleManager.Instance.ActionPile.AddActionToPile(navigationState.currentAction);
        BattleGridUI.Instance.ToggleRange(false);
    }
}