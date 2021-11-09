using UnityEngine;

public static class BattleGridUtils
{
    public static Vector3 TranslatedPosition(Vector2Int original, float heightDelta)
    {
        return new Vector3(original.x, heightDelta, original.y);
    }

    public static Vector3 TranslatedPosition(int x, int y, float heightDelta)
    {
        return new Vector3(x, heightDelta, y);
    }
}