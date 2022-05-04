using UnityEngine;
using System.Collections.Generic;
using System;

public static class BattleGridUtils
{
    public static float ShovingDamage { get { return 3.0f; } }
    public static float DefenseSplitFactor { get { return 0.5f; } }
    public static float BossCriticalDamageMultiplier { get { return 1.25f; } }
    public static float CharacterCriticalDamageMultiplier { get { return 1.5f; } }

    public static Vector3 TranslatedPosition(Vector2Int original, float heightDelta)
    {
        return new Vector3(original.x, heightDelta, original.y);
    }

    public static Vector3 TranslatedPosition(int x, int y, float heightDelta)
    {
        return new Vector3(x, heightDelta, y);
    }

    public static Vector2Int GridPosition(Vector3 worldPosition)
    {
        return new Vector2Int((int)worldPosition.x, (int)worldPosition.z);
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

    public static List<Vector2Int> GetAdjacentPositions(Vector2Int center)
    {
        List<Vector2Int> adjacent = new List<Vector2Int>();
        for (int rowDelta = -1; rowDelta <= 1; rowDelta++)
        {
            for (int columnDelta = -1; columnDelta <= 1; columnDelta++)
            {
                Vector2Int delta = new Vector2Int(columnDelta, rowDelta);
                if (delta == Vector2Int.zero)
                    continue;

                Vector2Int position = center + delta;
                if (!BattleManager.Instance.BattleGrid.IsInsideGrid(position.x, position.y))
                    continue;

                adjacent.Add(position);
            }
        }
        return adjacent;
    }
}