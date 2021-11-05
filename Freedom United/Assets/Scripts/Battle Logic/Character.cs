using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    private Vector2Int currentPosition;
    public Vector2Int CurrentPosition { get { return currentPosition; } }

    private CharacterID characterID;
    public CharacterID CharacterID { get { return characterID; } }

    public Character(CharacterID charID)
    {
        characterID = charID;
    }

    public void MoveToPosition(Vector2Int newPosition)
    {
        currentPosition = newPosition;
    }

}
