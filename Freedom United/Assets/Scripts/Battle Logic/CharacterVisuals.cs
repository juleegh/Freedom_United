using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private CharacterAssets assets;
    [SerializeField] private DeathIcon deathIcon;

    public void Paint(CharacterID character)
    {
        body.sprite = assets.Bodies[character];
    }

    public void PaintDeath()
    {
        deathIcon.Died();
    }
}
