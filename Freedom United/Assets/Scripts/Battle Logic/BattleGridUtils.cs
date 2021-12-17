using UnityEngine;
using System;

public static class BattleGridUtils
{
    public static float ShovingDamage { get { return 3.0f; } }

    public static Vector3 TranslatedPosition(Vector2Int original, float heightDelta)
    {
        return new Vector3(original.x, heightDelta, original.y);
    }

    public static Vector3 TranslatedPosition(int x, int y, float heightDelta)
    {
        return new Vector3(x, heightDelta, y);
    }

    public static bool IsACharacter(string characterID)
    {
        try
        {
            CharacterID character = (CharacterID)System.Enum.Parse(typeof(CharacterID), characterID);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static CharacterID GetCharacterID(string characterID)
    {
        try
        {
            CharacterID character = (CharacterID)System.Enum.Parse(typeof(CharacterID), characterID);
            return character;
        }
        catch (Exception)
        {
            return CharacterID.Daphne;
        }
    }

    public static BossPartType GetBossPart(string bossPartID)
    {
        try
        {
            BossPartType bossPart = (BossPartType)System.Enum.Parse(typeof(BossPartType), bossPartID);
            return bossPart;
        }
        catch (Exception)
        {
            return BossPartType.RobotArm;
        }
    }

    public static int GetRangeConversion(AttackRange attackRange)
    {
        switch (attackRange)
        {
            case AttackRange.Short:
                return 1;
            case AttackRange.Medium:
                return 2;
            case AttackRange.Long:
                return 3;
        }

        return 0;
    }


    public static int GetActionSpeed()
    {
        NavigationCurrentState navigationState = BattleUINavigation.Instance.NavigationState;

        switch (navigationState.currentLevel)
        {
            case BattleSelectionLevel.Character:
            case BattleSelectionLevel.ActionPile:
            case BattleSelectionLevel.Action:
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
}