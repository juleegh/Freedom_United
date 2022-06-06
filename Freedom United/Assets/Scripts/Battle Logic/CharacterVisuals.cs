using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisuals : MonoBehaviour
{
    [SerializeField] private CharacterID characterID;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private CharacterAssets assets;
    [SerializeField] private CharacterStatsVisuals stats;
    [SerializeField] private DeathIcon deathIcon;

    public CharacterID CharacterID { get { return characterID; } }

    [ContextMenu("Refresh")]
    public void Paint()
    {
        body.sprite = assets.Bodies[characterID];
        deathIcon.Clear();
    }

    public void Initialize()
    {
        stats.Initialize(characterID.ToString());
    }

    public void PaintDeath()
    {
        deathIcon.Died();
    }
}
