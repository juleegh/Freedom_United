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

    [SerializeField] private int fovDepth;
    public int FoVDepth { get { return fovDepth; } }
    [SerializeField] private int fovWidth;
    public int FoVWidth { get { return fovWidth; } }

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

    //[ContextMenu("Fill Positions")]
    public void FillPositions()
    {
        foreach (BossPartConfig config in bossParts.Values)
        {
            config.FillPositions();
        }
    }
}


[Serializable]
public class BossParts : SerializableDictionaryBase<BossPartType, BossPartConfig> { }