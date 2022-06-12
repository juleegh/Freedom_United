using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationCurrentState
{
    public CharacterSelection CharacterSelection;
    public MagicSelection MagicSelection;
    public ActionPileSelection ActionPileSelection;
    public ActionSelection ActionSelection;
    public CellSelection CellSelection;

    public BattleSelectionLevel currentLevel;

    public AllyAction currentAction;

    public NavigationCurrentState()
    {
        CharacterSelection = new CharacterSelection();
        MagicSelection = new MagicSelection();
        ActionPileSelection = new ActionPileSelection();
        ActionSelection = new ActionSelection();
        CellSelection = new CellSelection();
    }

    public void ClearActionSelection()
    {
        currentAction = new AllyAction();
        currentLevel = BattleSelectionLevel.Character;
        CellSelection.Toggle(false);
        ActionSelection.Toggle(false);
        MagicSelection.Toggle(false);
    }
    
    public void ResetActionSelection()
    {
        ClearActionSelection();
        CharacterSelection.Refresh();
    }
}