using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Attack Random Character")]
public class AIAttackRandomCharacter : AIAttackAction
{
    protected override void AddToActionPile()
    {
        bool result = SelectRandomTarget();
        if (!result)
            SelectCounterPartTarget();
    }
}
