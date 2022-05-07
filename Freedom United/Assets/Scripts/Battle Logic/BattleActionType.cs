using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleActionType
{
    Attack,
    Defend,
    MoveSafely,
    MoveFast,
    Magic,
    Rotate,
    SuicideAttack,
}

public enum TargetType
{
    Empty,
    Character,
    BossPart,
}

public static class BattleActionsUtils
{
    public static BattleActionType GetByIndex(int index)
    {
        switch (index)
        {
            case 0:
                return BattleActionType.Attack;
            case 1:
                return BattleActionType.Defend;
            case 2:
                return BattleActionType.MoveSafely;
            case 3:
                return BattleActionType.MoveFast;
            case 4:
                return BattleActionType.Magic;
        }

        return BattleActionType.Attack;
    }

    public static List<BattleActionType> GetActionsList()
    {
        List<BattleActionType> actionsAvailable = new List<BattleActionType>();
        actionsAvailable.Add(BattleActionType.Attack);
        actionsAvailable.Add(BattleActionType.Defend);
        actionsAvailable.Add(BattleActionType.MoveSafely);
        actionsAvailable.Add(BattleActionType.MoveFast);
        actionsAvailable.Add(BattleActionType.Magic);
        return actionsAvailable;
    }

    public static int GetActionSpeed()
    {
        NavigationCurrentState navigationState = BattleUINavigation.Instance.NavigationState;

        switch (navigationState.currentLevel)
        {
            case BattleSelectionLevel.Character:
            case BattleSelectionLevel.ActionPile:
                return 0;
        }

        switch (navigationState.ActionSelection.ActionSelected)
        {
            case BattleActionType.Attack:
                return BattleManager.Instance.PartyStats.Stats[navigationState.CharacterSelection.CharacterID].AttackSpeed;
            case BattleActionType.Defend:
                return BattleManager.Instance.PartyStats.Stats[navigationState.CharacterSelection.CharacterID].DefenseSpeed;
            case BattleActionType.MoveFast:
                return BattleManager.Instance.PartyStats.Stats[navigationState.CharacterSelection.CharacterID].RecklessChangeSpeed;
            case BattleActionType.MoveSafely:
                return BattleManager.Instance.PartyStats.Stats[navigationState.CharacterSelection.CharacterID].SafeChangeSpeed;
        }

        return 0;
    }

    public static TargetType GetTargetType(string targetId)
    {
        if (BattleGridUtils.IsACharacter(targetId))
            return TargetType.Character;
        else if (BattleGridUtils.IsABossPart(targetId))
            return TargetType.BossPart;

        return TargetType.Empty;
    }
}