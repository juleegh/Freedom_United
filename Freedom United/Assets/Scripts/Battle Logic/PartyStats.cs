using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;

[CreateAssetMenu(fileName = "Party Stats")]
public class PartyStats : ScriptableObject
{
    [Serializable]
    public class CharacterStatsValues : SerializableDictionaryBase<CharacterID, CharacterStats> { }

    [SerializeField] private CharacterStatsValues stats;
    public CharacterStatsValues Stats { get { return stats; } }
}
