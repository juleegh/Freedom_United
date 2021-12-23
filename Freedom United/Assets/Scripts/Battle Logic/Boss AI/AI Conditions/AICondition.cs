using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICondition : ScriptableObject
{
    public virtual bool MeetsRequirement()
    {
        return true;
    }
}

