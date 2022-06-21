using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCellPreview : MonoBehaviour
{
    [SerializeField] private SpriteRenderer character;
    [SerializeField] private SpriteRenderer attack;
    [SerializeField] private SpriteRenderer defense;
    [SerializeField] private CharacterAssets assets;

    public void ClearPreview()
    {
        character.gameObject.SetActive(false);
        attack.gameObject.SetActive(false);
        defense.gameObject.SetActive(false);
    }

    public void CreateCharacterPreview(CharacterID characterID)
    {
        character.gameObject.SetActive(true);
        character.sprite = assets.Bodies[characterID];
    }

    public void CreateAttackPreview()
    {
        attack.gameObject.SetActive(true);
    }

    public void CreateDefensePreview()
    { 
        defense.gameObject.SetActive(true);
    }
}
