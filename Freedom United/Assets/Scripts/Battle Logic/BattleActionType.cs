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
}