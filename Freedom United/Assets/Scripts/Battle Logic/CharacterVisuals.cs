using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private CharacterAssets assets;

    public void Paint(CharacterID character)
    {
        body.sprite = assets.Bodies[character];
    }
}
