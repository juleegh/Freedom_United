using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[CreateAssetMenu(fileName = "Character Assets")]
public class CharacterAssets : ScriptableObject
{
    [Serializable]
    public class BodiesDictionary : SerializableDictionaryBase<CharacterID, Sprite> { }

    [SerializeField] private BodiesDictionary bodies;
    public BodiesDictionary Bodies { get { return bodies; } }
}