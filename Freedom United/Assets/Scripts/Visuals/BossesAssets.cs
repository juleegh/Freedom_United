using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[CreateAssetMenu(fileName = "Boss Assets")]
public class BossesAssets : ScriptableObject
{
    [Serializable]
    public class PartsDictionary : SerializableDictionaryBase<BossPartType, Sprite> { }

    [SerializeField] private PartsDictionary parts;
    public PartsDictionary Parts { get { return parts; } }
}