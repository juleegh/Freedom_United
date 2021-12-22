using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManagement : MonoBehaviour, NotificationsListener
{
    [SerializeField] private List<Vector2Int> initialPositions;
    [SerializeField] private Vector2Int bossInitialPosition;
    [SerializeField] private BossConfig bossConfig;
    private Dictionary<CharacterID, Character> characters;
    public Dictionary<CharacterID, Character> Characters { get { return characters; } }
    private Boss boss;
    public Boss Boss { get { return boss; } }
    public BossConfig BossConfig { get { return bossConfig; } }

    public void ConfigureComponent()
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

        boss = new Boss(bossConfig);
    }

    public void SetCharacterInPosition(CharacterID characterID, Vector2Int newPosition)
    {
        characters[characterID].MoveToPosition(newPosition);
    }


    public BossPart GetBossPartInPosition(Vector2Int position)
    {

        foreach (BossPart bossPart in boss.Parts.Values)
        {
            if (bossPart.OccupiesPosition(position.x, position.y))
                return bossPart;
        }

        return null;
    }

    public Character GetCharacterInPosition(Vector2Int position)
    {
        foreach (Character character in characters.Values)
        {
            if (character.CurrentPosition == position)
                return character;
        }

        return null;
    }
}
