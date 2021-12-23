using UnityEngine;
using System;

public static class BattleGridUtils
{
    public static float ShovingDamage { get { return 3.0f; } }
    public static float DefenseSplitFactor { get { return 0.5f; } }

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

    public static bool IsABossPart(string partID)
    {
        try
        {
            BossPartType bossPart = (BossPartType)System.Enum.Parse(typeof(BossPartType), partID);
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
            return CharacterID.Noma;
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
}