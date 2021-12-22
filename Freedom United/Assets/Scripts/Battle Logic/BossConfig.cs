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

    [SerializeField] private float baseHealth;
    public float BaseHealth { get { return baseHealth; } }

    [SerializeField] private BossParts bossParts;
    public BossParts PartsList { get { return bossParts; } }

    [SerializeField] private CharacterID characterCounterPart;
    public CharacterID CharacterCounterPart { get { return characterCounterPart; } }

}

[Serializable]
public class BossPartConfig
{
    [SerializeField] private BossPartType partType;
    [SerializeField] private float baseDurability;
    [SerializeField] private bool isCore;

    [SerializeField] private Vector2Int position;
    [SerializeField] private Vector2Int dimensions;
    [SerializeField] private int attackSpeed;
    [SerializeField] private int defenseSpeed;

    [SerializeField] private List<AreaOfEffect> areasOfEffect;
    [SerializeField] private BossDefenseType bossDefenseType;
    [SerializeField] private List<BossPartType> defendedParts;

    public BossPartType PartType { get { return partType; } }
    public float BaseDurability { get { return baseDurability; } }
    public bool IsCore { get { return isCore; } }
    public Vector2Int Position { get { return position; } }
    public Vector2Int Dimensions { get { return dimensions; } }
    public List<AreaOfEffect> AreasOfEffect { get { return areasOfEffect; } }
    public int AttackSpeed { get { return attackSpeed; } }
    public int DefenseSpeed { get { return defenseSpeed; } }
    public BossDefenseType BossDefenseType { get { return bossDefenseType; } }
    public List<BossPartType> DefendedParts { get { return defendedParts; } }
}

[Serializable]
public class AreaOfEffect
{
    [SerializeField] private List<Vector2Int> positions;
    public AreaOfEffect(List<Vector2Int> pos) { positions = pos; }
    public List<Vector2Int> Positions { get { return positions; } }
}

[Serializable]
public class BossParts : SerializableDictionaryBase<BossPartType, BossPartConfig> { }