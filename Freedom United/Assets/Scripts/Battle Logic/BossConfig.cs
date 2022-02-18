using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[CreateAssetMenu(fileName = "Boss Config")]
public class BossConfig : ScriptableObject
{
    [SerializeField] private string bossName;
    public string BossName { get { return bossName; } }

    [SerializeField] private BossParts bossParts;
    public BossParts PartsList { get { return bossParts; } }

    [SerializeField] private CharacterID characterCounterPart;
    public CharacterID CharacterCounterPart { get { return characterCounterPart; } }

    public float BaseHealth
    {
        get
        {
            float totalHealth = 0;
            foreach (BossPartConfig part in bossParts.Values)
            {
                totalHealth += part.BaseDurability;
            }
            return totalHealth;
        }
    }

}

[Serializable]
public class BossPartConfig
{
    [SerializeField] private BossPartType partType;
    [SerializeField] private float baseDurability;
    [SerializeField] private bool isCore;
    [SerializeField] private bool rotatesWithBody;

    [SerializeField] private Vector2Int position;
    [SerializeField] private Vector2Int dimensions;
    [SerializeField] private float baseAttack;
    [SerializeField] private float baseDefense;
    [SerializeField] private int attackSpeed;
    [SerializeField] private int defenseSpeed;

    [SerializeField] private List<AreaOfEffect> areasOfEffect;
    [SerializeField] private BossDefenseType bossDefenseType;
    [SerializeField] private List<BossPartType> defendedParts;

    [SerializeField] private float normalSuccessChance;
    [SerializeField] private float criticalSuccessChance;
    [SerializeField] private float criticalFailureChance;

    public BossPartType PartType { get { return partType; } }
    public float BaseDurability { get { return baseDurability; } }
    public float BaseAttack { get { return baseAttack; } }
    public float BaseDefense { get { return baseDefense; } }
    public bool IsCore { get { return isCore; } }
    public bool RotatesWithBody { get { return rotatesWithBody; } }
    public Vector2Int InitialPosition { get { return position; } }
    public Vector2Int Dimensions { get { return dimensions; } }
    public List<AreaOfEffect> AreasOfEffect { get { return areasOfEffect; } }
    public int AttackSpeed { get { return attackSpeed; } }
    public int DefenseSpeed { get { return defenseSpeed; } }
    public BossDefenseType BossDefenseType { get { return bossDefenseType; } }
    public List<BossPartType> DefendedParts { get { return defendedParts; } }
    public float NormalSuccessChance { get { return normalSuccessChance; } }
    public float CriticalSuccessChance { get { return criticalFailureChance; } }
    public float CriticalFailureChance { get { return criticalFailureChance; } }
}

[Serializable]
public class AreaOfEffect
{
    [SerializeField] private List<Vector2Int> positions;
    public AreaOfEffect(List<Vector2Int> pos) { positions = pos; }
    public List<Vector2Int> Positions { get { return positions; } }

    public bool PositionInsideArea(Vector2Int targetPosition, Vector2Int pivotPosition, Vector2Int orientation)
    {
        foreach (Vector2Int pos in positions)
        {
            Vector2Int affectedPosition = BossUtils.GetOrientedTransformation(orientation, pos.x, pos.y) + pivotPosition;
            if (affectedPosition == targetPosition)
                return true;
        }
        return false;
    }

    public List<Vector2Int> GetPositions(Vector2Int pivot, Vector2Int orientation)
    {
        List<Vector2Int> transformed = new List<Vector2Int>();
        foreach (Vector2Int pos in positions)
        {
            Vector2Int affectedPosition = pivot + BossUtils.GetOrientedTransformation(orientation, pos.x, pos.y);
            transformed.Add(affectedPosition);
        }
        return transformed;
    }
}

[Serializable]
public class BossParts : SerializableDictionaryBase<BossPartType, BossPartConfig> { }