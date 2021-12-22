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

//[CreateAssetMenu(fileName = "AIC No Player Attacks")]
public class AIC_NoPlayerAttacks : AICondition
{
    public override bool MeetsRequirement()
    {
        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.Registers)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionOwner) == TargetType.Character)
            {
                if (actionInfo.actionType == BattleActionType.Attack)
                    return false;
            }
        }

        return true;
    }
}

//[CreateAssetMenu(fileName = "AIC Counter Part HP Below Half")]
public class AIC_CounterPartHPBelowHalf : AICondition
{
    public override bool MeetsRequirement()
    {
        CharacterID counterPart = BattleManager.Instance.CharacterManagement.BossConfig.CharacterCounterPart;
        return BattleManager.Instance.BattleValues.PartyHealth[counterPart] < BattleManager.Instance.PartyStats.Stats[counterPart].BaseHealth * 0.5f;
    }
}

//[CreateAssetMenu(fileName = "AIC Counter Part Performed Failure")]
public class AIC_CounterPartPerformedFailure : AICondition
{
    public override bool MeetsRequirement()
    {
        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.Registers)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionOwner) == TargetType.Character && BattleGridUtils.GetCharacterID(actionInfo.actionOwner) == BattleManager.Instance.CharacterManagement.BossConfig.CharacterCounterPart)
            {
                if (actionInfo.wasFailure)
                    return true;
            }
        }

        return false;
    }
}

//[CreateAssetMenu(fileName = "AIC Boss Performed Failure")]
public class AIC_BossPerformedFailure : AICondition
{
    public override bool MeetsRequirement()
    {
        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.Registers)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionOwner) == TargetType.BossPart)
            {
                if (actionInfo.wasFailure)
                    return true;
            }
        }

        return false;
    }
}

//[CreateAssetMenu(fileName = "AIC Boss HP Below Half")]
public class AIC_BossHPBelowHalf : AICondition
{
    public override bool MeetsRequirement()
    {
        return BattleManager.Instance.BattleValues.BossHealth < BattleManager.Instance.CharacterManagement.BossConfig.BaseHealth * 0.5f;
    }
}

//[CreateAssetMenu(fileName = "AIC Boss Part Destroyed")]
public class AIC_BossPartDestroyed : AICondition
{
    public override bool MeetsRequirement()
    {
        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.Registers)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionTarget) == TargetType.BossPart)
            {
                if (actionInfo.previousTargetHP > 0f && actionInfo.newTargetHP <= 0f)
                    return true;
            }
        }

        return false;
    }
}
