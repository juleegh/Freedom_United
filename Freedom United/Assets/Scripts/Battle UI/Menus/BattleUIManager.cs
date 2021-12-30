using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : MonoBehaviour, NotificationsListener
{
    private static BattleUIManager instance;
    public static BattleUIManager Instance { get { return instance; } }

    [SerializeField] private CharacterSelectionUI characterSelectionUI;
    public CharacterSelectionUI CharacterSelectionUI { get { return characterSelectionUI; } }

    [SerializeField] private ActionSelectionUI actionSelectionUI;
    public ActionSelectionUI ActionSelectionUI { get { return actionSelectionUI; } }

    [SerializeField] private CancelUI cancelUI;
    public CancelUI CancelUI { get { return cancelUI; } }

    [SerializeField] private ActionPileUI actionPileUI;
    public ActionPileUI ActionPileUI { get { return actionPileUI; } }

    [SerializeField] private MagicSelectionUI magicSelectionUI;
    public MagicSelectionUI MagicSelectionUI { get { return magicSelectionUI; } }

    [SerializeField] private CellSelectionUI cellSelectionUI;
    public CellSelectionUI CellSelectionUI { get { return cellSelectionUI; } }

    public void ConfigureComponent()
    {
        instance = this;
    }
}
