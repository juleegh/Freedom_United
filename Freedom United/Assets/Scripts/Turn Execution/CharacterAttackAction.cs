using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAttackAction : ExecutingAction
{
    private CharacterID attackingCharacter;
    private List<Vector2Int> attackedPositions;
    private int attackDelta;
    private Vector2Int attackDirection;

    private float defenseProvided;
    public float DefenseProvided { get { return defenseProvided; } }
    private int damageTaken;

    private float chanceResult;

    public CharacterAttackAction(AllyAction scheduledAction) : base(scheduledAction)
    {
        attackingCharacter = scheduledAction.actionOwner;
        attackDelta = BattleGridUtils.GetRangeConversion(BattleManager.Instance.PartyStats.Stats[scheduledAction.actionOwner].AttackRange);
        attackDirection = scheduledAction.position;
        attackDirection.x = attackDirection.x == 0 ? 0 : attackDirection.x / Mathf.Abs(attackDirection.x);
        attackDirection.y = attackDirection.y == 0 ? 0 : attackDirection.y / Mathf.Abs(attackDirection.y);

        attackedPositions = new List<Vector2Int>();
        for (int i = 1; i <= attackDelta; i++)
        {
            attackedPositions.Add(attackDirection * i);
        }

        damageTaken = BattleManager.Instance.PartyStats.Stats[scheduledAction.actionOwner].BaseAttack;
        chanceResult = Random.Range(0f, 1f);
    }

    public override void Execute()
    {
        foreach (Vector2Int position in attackedPositions)
        {
            Vector2Int attackedPosition = position + BattleManager.Instance.CharacterManagement.Characters[attackingCharacter].CurrentPosition;

            GameNotificationData attackData = new GameNotificationData();
            attackData.Data[NotificationDataIDs.ActionOwner] = attackingCharacter.ToString();
            attackData.Data[NotificationDataIDs.Failure] = FailedSuccess();
            attackData.Data[NotificationDataIDs.Critical] = PassedCritical();
            attackData.Data[NotificationDataIDs.ActionTarget] = "empty";
            attackData.Data[NotificationDataIDs.NewHP] = 0f;
            attackData.Data[NotificationDataIDs.PreviousHP] = 0f;
            attackData.Data[NotificationDataIDs.CellPosition] = attackedPosition;

            float defenseInPosition = TurnExecutor.Instance.DefenseValueInPosition(attackedPosition);
            float damageProvided = damageTaken - defenseInPosition;
            damageProvided = Mathf.Clamp(damageProvided, 0, damageProvided);
            if (FailedSuccess())
                damageProvided = 0;
            else if (PassedCritical())
                damageProvided *= BattleGridUtils.CharacterCriticalDamageMultiplier;

            if (BattleManager.Instance.CharacterManagement.GetBossPartInPosition(attackedPosition) != null)
            {
                BossPart targetPart = BattleManager.Instance.CharacterManagement.GetBossPartInPosition(attackedPosition);
                if (BattleManager.Instance.BattleValues.BossPartIsDestroyed(targetPart.PartType))
                    continue;

                attackData.Data[NotificationDataIDs.ActionTarget] = targetPart.ToString();

                attackData.Data[NotificationDataIDs.PreviousHP] = BattleManager.Instance.BattleValues.BossPartsHealth[targetPart.PartType];
                BattleManager.Instance.BattleValues.BossTakeDamage(targetPart.PartType, damageProvided);
                attackData.Data[NotificationDataIDs.NewHP] = BattleManager.Instance.BattleValues.BossPartsHealth[targetPart.PartType];
                GameNotificationsManager.Instance.Notify(GameNotification.BossStatsModified);

                if (BattleManager.Instance.BattleValues.BossPartIsDestroyed(targetPart.PartType))
                    BattleManager.Instance.BattleValues.CharacterModifyWillPower(attackingCharacter, BattleGridUtils.DestroyingBodyPartWillPercentage);
            }
            else if (BattleManager.Instance.CharacterManagement.GetCharacterInPosition(attackedPosition) != null)
            {
                Character targetCharacter = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(attackedPosition);
                attackData.Data[NotificationDataIDs.ActionTarget] = targetCharacter.ToString();

                attackData.Data[NotificationDataIDs.PreviousHP] = BattleManager.Instance.BattleValues.PartyHealth[targetCharacter.CharacterID];
                BattleManager.Instance.BattleValues.CharacterTakeDamage(targetCharacter.CharacterID, damageProvided);
                attackData.Data[NotificationDataIDs.NewHP] = BattleManager.Instance.BattleValues.PartyHealth[targetCharacter.CharacterID];
            }

            GameNotificationsManager.Instance.Notify(GameNotification.AttackWasExecuted, attackData);

        }

        if (FailedSuccess())
        {
            BattleManager.Instance.BattleValues.CharacterModifyWillPower(attackingCharacter, BattleGridUtils.FailureWillPercentage);
        }
        else if (PassedCritical())
        {
            BattleManager.Instance.BattleValues.CharacterModifyWillPower(attackingCharacter, BattleGridUtils.CriticalWillPercentage);
        }
    }

    private bool FailedSuccess()
    {
        return chanceResult <= BattleManager.Instance.PartyStats.Stats[attackingCharacter].CriticalFailureChance;
    }

    private bool PassedCritical()
    {
        float failure = BattleManager.Instance.PartyStats.Stats[attackingCharacter].CriticalFailureChance;
        float success = BattleManager.Instance.PartyStats.Stats[attackingCharacter].NormalSuccessChance;
        Debug.LogError(chanceResult + " - " + (failure + success));
        return chanceResult >= failure + success;
    }
}