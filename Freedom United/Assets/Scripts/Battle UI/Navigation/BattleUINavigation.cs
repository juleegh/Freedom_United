using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUINavigation : MonoBehaviour
{
    private BattleSelectionLevel currentLevel;
    private CharacterSelection CharacterSelection = new CharacterSelection();
    private MagicSelection MagicSelection = new MagicSelection();
    private ActionPileSelection ActionPileSelection = new ActionPileSelection();
    private ActionSelection ActionSelection = new ActionSelection();
    private CellSelection CellSelection = new CellSelection();

    void Start()
    {
        ResetActionSelection();
    }

    public void Down()
    {
        if (currentLevel == BattleSelectionLevel.ActionPile)
            ActionPileSelection.Next();
        if (currentLevel == BattleSelectionLevel.Action)
            ActionSelection.Next();
        if (currentLevel == BattleSelectionLevel.Magic)
            MagicSelection.Next();
        if (currentLevel == BattleSelectionLevel.Cell)
            CellSelection.Down();
    }

    public void Up()
    {
        if (currentLevel == BattleSelectionLevel.ActionPile)
            ActionPileSelection.Previous();
        if (currentLevel == BattleSelectionLevel.Action)
            ActionSelection.Previous();
        if (currentLevel == BattleSelectionLevel.Magic)
            MagicSelection.Previous();
        if (currentLevel == BattleSelectionLevel.Cell)
            CellSelection.Up();
    }

    public void Left()
    {
        if (currentLevel == BattleSelectionLevel.Character)
            CharacterSelection.Previous();
        else if (currentLevel == BattleSelectionLevel.Cell)
            CellSelection.Left();
    }

    public void Right()
    {
        if (currentLevel == BattleSelectionLevel.Character)
            CharacterSelection.Next();
        else if (currentLevel == BattleSelectionLevel.Cell)
            CellSelection.Right();
    }

    public void Forward()
    {
        if (currentLevel == BattleSelectionLevel.ActionPile)
            currentLevel = BattleSelectionLevel.Character;
        else if (currentLevel == BattleSelectionLevel.Character)
        {
            currentLevel = BattleSelectionLevel.Action;
            ActionSelection.Toggle(true);
        }
        else if (currentLevel == BattleSelectionLevel.Action)
        {
            if (ActionSelection.MagicSelected)
            {
                currentLevel = BattleSelectionLevel.Magic;
                MagicSelection.Toggle(true);
            }
            else
            {
                currentLevel = BattleSelectionLevel.Cell;
                Vector2Int position = CharacterSelection.SelectedCharacter.CurrentPosition;
                CellSelection.Toggle(true);
                CellSelection.Initialize(position);
            }
        }
        else if (currentLevel == BattleSelectionLevel.Magic)
        {
            currentLevel = BattleSelectionLevel.Cell;
            CellSelection.Toggle(true);
        }
        else if (currentLevel == BattleSelectionLevel.Cell)
            ResetActionSelection();
    }

    public void Backwards()
    {
        if (currentLevel == BattleSelectionLevel.Character)
            currentLevel = BattleSelectionLevel.ActionPile;
        else if (currentLevel == BattleSelectionLevel.Action)
        {
            currentLevel = BattleSelectionLevel.Character;
            ActionSelection.Toggle(false);
        }
        else if (currentLevel == BattleSelectionLevel.Magic)
        {
            currentLevel = BattleSelectionLevel.Action;
            MagicSelection.Toggle(false);
        }
        else if (currentLevel == BattleSelectionLevel.Cell)
        {
            currentLevel = BattleSelectionLevel.Action;
            MagicSelection.Toggle(false);
            CellSelection.Toggle(false);
        }
    }

    private void ResetActionSelection()
    {
        currentLevel = BattleSelectionLevel.Character;
        CellSelection.Toggle(false);
        ActionSelection.Toggle(false);
        MagicSelection.Toggle(false);
    }
}
