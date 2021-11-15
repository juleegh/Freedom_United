using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    private Vector2Int currentPosition;
    public Vector2Int CurrentPosition { get { return currentPosition; } }

    private Vector2Int currentOrientation;
    public Vector2Int CurrentOrientation { get { return currentOrientation; } }

    private CharacterID characterID;
    public CharacterID CharacterID { get { return characterID; } }

    public Character(CharacterID charID)
    {
        characterID = charID;
        currentOrientation = Vector2Int.up;
    }

    public void MoveToPosition(Vector2Int newPosition)
    {
        currentPosition = newPosition;
    }

}
