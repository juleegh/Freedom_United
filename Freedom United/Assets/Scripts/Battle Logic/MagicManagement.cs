using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManagement : MonoBehaviour
{
    private List<MagicSpell> spells;
    public List<MagicSpell> Spells { get { return spells; } }

    void Awake()
    {
        spells = new List<MagicSpell>();
        spells.Add(new MagicSpell("Fire"));
        spells.Add(new MagicSpell("Fira"));
        spells.Add(new MagicSpell("Firaga"));
    }
}
