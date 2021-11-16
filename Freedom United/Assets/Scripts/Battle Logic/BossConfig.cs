using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[CreateAssetMenu(fileName = "Boss Config")]
public class BossConfig : ScriptableObject
{

    [Serializable]
    public class BossParts : SerializableDictionaryBase<BossPartType, BossPartConfig> { }

    [SerializeField] private BossParts bossParts;
    public BossParts PartsList { get { return bossParts; } }
}

[Serializable]
public class BossPartConfig
{
    [SerializeField] private Vector2Int position;
    [SerializeField] private Vector2Int dimensions;

    [SerializeField] private List<Vector2Int> attackPositions;
    [SerializeField] private BossDefenseType bossDefenseType;

    public Vector2Int Position { get { return position; } }
    public Vector2Int Dimensions { get { return dimensions; } }
}