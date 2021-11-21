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

}

[Serializable]
public class BossPartConfig
{
    [SerializeField] private float baseDurability;
    [SerializeField] private bool isCore;

    [SerializeField] private Vector2Int position;
    [SerializeField] private Vector2Int dimensions;

    [SerializeField] private List<AreaOfEffect> areasOfEffect;
    [SerializeField] private BossDefenseType bossDefenseType;
    [SerializeField] private List<BossPartType> defendedParts;

    public float BaseDurability { get { return baseDurability; } }
    public bool IsCore { get { return isCore; } }
    public Vector2Int Position { get { return position; } }
    public Vector2Int Dimensions { get { return dimensions; } }
}

[Serializable]
public class AreaOfEffect
{
    [SerializeField] private List<Vector2Int> positions;
}

[Serializable]
public class BossParts : SerializableDictionaryBase<BossPartType, BossPartConfig> { }