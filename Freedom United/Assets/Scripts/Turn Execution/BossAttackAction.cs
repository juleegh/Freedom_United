using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossAttackAction : ExecutingAction
{
    private BossPartType attackingPart;
    private List<Vector2Int> attackedPositions;

    private float defenseProvided;
    public float DefenseProvided { get { return defenseProvided; } }
    private float damageTaken;

    private float chanceResult;

    public BossAttackAction(BossAction scheduledAction) : base(scheduledAction)
    {
        attackingPart = scheduledAction.actionOwner;
        attackedPositions = scheduledAction.areaOfEffect.Positions;
        damageTaken = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[attackingPart].BaseAttack;
        chanceResult = Random.Range(0f, 1f);
    }

    public override void Execute()
    {
        foreach (Vector2Int attackedPosition in attackedPositions)
        {
            GameNotificationData attackData = new GameNotificationData();

            attackData.Data[NotificationDataIDs.ActionOwner] = attackingPart.ToString();
            attackData.Data[NotificationDataIDs.Failure] = FailedSuccess();
            attackData.Data[NotificationDataIDs.ActionTarget] = "empty";
            attackData.Data[NotificationDataIDs.NewHP] = 0f;
            attackData.Data[NotificationDataIDs.PreviousHP] = 0f;

            float defenseInPosition = TurnExecutor.Instance.DefenseValueInPosition(attackedPosition);
            float damageProvided = damageTaken - defenseInPosition;
            damageProvided = Mathf.Clamp(damageProvided, 0, damageProvided);
            if (FailedSuccess())
                damageProvided = 0;

            if (BattleManager.Instance.CharacterManagement.GetBossPartInPosition(attackedPosition) != null)
            {
                BossPart targetPart = BattleManager.Instance.CharacterManagement.GetBossPartInPosition(attackedPosition);
                attackData.Data[NotificationDataIDs.ActionTarget] = targetPart.ToString();

                attackData.Data[NotificationDataIDs.PreviousHP] = BattleManager.Instance.BattleValues.BossPartsHealth[targetPart.PartType];
                BattleManager.Instance.BattleValues.BossTakeDamage(targetPart.PartType, damageProvided);
                attackData.Data[NotificationDataIDs.NewHP] = BattleManager.Instance.BattleValues.BossPartsHealth[targetPart.PartType];
                GameNotificationsManager.Instance.Notify(GameNotification.BossStatsModified);
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
    }

    private bool FailedSuccess()
    {
        return chanceResult <= BattleManager.Instance.CharacterManagement.BossConfig.PartsList[attackingPart].CriticalFailureChance;
    }

    private bool PassedCritical()
    {
        float failure = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[attackingPart].CriticalFailureChance;
        float success = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[attackingPart].NormalSuccessChance;
        return chanceResult + failure + success >= BattleManager.Instance.CharacterManagement.BossConfig.PartsList[attackingPart].CriticalSuccessChance;
    }

}