using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Attack Counter Part")]
public class AIAttackCounterPart : AIAttackAction
{
    protected override void AddToActionPile()
    {
        bool result = SelectCounterPartTarget();
        if (!result)
            SelectRandomTarget();
    }
}
