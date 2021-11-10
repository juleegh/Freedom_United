using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance;
    public static BattleManager Instance { get { return instance; } }

    private BattleGrid battleGrid;
    public BattleGrid BattleGrid { get { return battleGrid; } }

    private CharacterManagement characterManagement;
    public CharacterManagement CharacterManagement { get { return characterManagement; } }

    private ActionPile actionPile;
    public ActionPile ActionPile { get { return actionPile; } }

    private MagicManagement magicManagement;
    public MagicManagement MagicManagement { get { return magicManagement; } }

    private void Awake()
    {
        instance = this;
        battleGrid = GetComponent<BattleGrid>();
        characterManagement = GetComponent<CharacterManagement>();
        actionPile = GetComponent<ActionPile>();
        magicManagement = GetComponent<MagicManagement>();
    }
}
