using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManagement : MonoBehaviour
{
    [SerializeField] private List<Vector2Int> initialPositions;
    private Dictionary<CharacterID, Character> characters;
    public Dictionary<CharacterID, Character> Characters { get { return characters; } }

    void Awake()
    {
        InitializeCharacters();
    }

    private void InitializeCharacters()
    {
        characters = new Dictionary<CharacterID, Character>();
        characters.Add(CharacterID.Daphne, new Character(CharacterID.Daphne));
        characters.Add(CharacterID.Simon, new Character(CharacterID.Simon));
        characters.Add(CharacterID.Anthony, new Character(CharacterID.Anthony));

        SetCharacterInPosition(CharacterID.Daphne, initialPositions[0]);
        SetCharacterInPosition(CharacterID.Simon, initialPositions[1]);
        SetCharacterInPosition(CharacterID.Anthony, initialPositions[2]);
    }

    public void SetCharacterInPosition(CharacterID characterID, Vector2Int newPosition)
    {
        characters[characterID].MoveToPosition(newPosition);
    }
}
