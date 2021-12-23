using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI_Attack_Default_Action")]
public class AIAttackDefault : AIAttackAction
{
    protected override void AddToActionPile()
    {
        AddToPileDefault();
    }
}
