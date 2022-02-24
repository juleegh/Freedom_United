using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossAttackAction : ExecutingAction
{
    private BossPartType attackingPart;
    private SetOfPositions selectedArea;

    private float damageTaken;

    private float chanceResult;

    public BossAttackAction(BossAction scheduledAction) : base(scheduledAction)
    {
        attackingPart = scheduledAction.actionOwner;
        damageTaken = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[attackingPart].BaseAttack;
        selectedArea = scheduledAction.deltaOfAction;
        chanceResult = Random.Range(0f, 1f);
    }

    public override void Execute()
    {
        Vector2Int currentPartPosition = BattleManager.Instance.CharacterManagement.Boss.Parts[attackingPart].Position;
        Vector2Int currentPartOrientation = BattleManager.Instance.CharacterManagement.Boss.Parts[attackingPart].Orientation;
        List<Vector2Int> attackedPositions = selectedArea.GetPositions(currentPartPosition, currentPartOrientation);

        foreach (Vector2Int attackedPosition in attackedPositions)
        {
            GameNotificationData attackData = new GameNotificationData();

            attackData.Data[NotificationDataIDs.ActionOwner] = attackingPart.ToString();
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
                damageProvided *= BattleGridUtils.BossCriticalDamageMultiplier;

            if (BattleManager.Instance.CharacterManagement.GetBossPartInPosition(attackedPosition) != null)
            {
                BossPart targetPart = BattleManager.Instance.CharacterManagement.GetBossPartInPosition(attackedPosition);

                if (!BattleManager.Instance.BattleValues.BossPartIsDestroyed(targetPart.PartType))
                {
                    attackData.Data[NotificationDataIDs.ActionTarget] = targetPart.ToString();
                    attackData.Data[NotificationDataIDs.PreviousHP] = BattleManager.Instance.BattleValues.BossPartsHealth[targetPart.PartType];
                    BattleManager.Instance.BattleValues.BossTakeDamage(targetPart.PartType, damageProvided);
                    attackData.Data[NotificationDataIDs.NewHP] = BattleManager.Instance.BattleValues.BossPartsHealth[targetPart.PartType];
                    GameNotificationsManager.Instance.Notify(GameNotification.BossStatsModified);

                    if (BattleManager.Instance.BattleValues.BossPartIsDestroyed(targetPart.PartType))
                        BattleManager.Instance.BattleGrid.AddPartObstacle(targetPart.PartType);
                }
                else if (BattleManager.Instance.BattleGrid.GetObstacleHP(attackedPosition) > 0)
                {
                    attackData.Data[NotificationDataIDs.ActionTarget] = "Obstacle";
                    attackData.Data[NotificationDataIDs.PreviousHP] = BattleManager.Instance.BattleGrid.GetObstacleHP(attackedPosition);
                    BattleManager.Instance.BattleGrid.HitObstacle(attackedPosition, damageProvided);
                    attackData.Data[NotificationDataIDs.NewHP] = BattleManager.Instance.BattleGrid.GetObstacleHP(attackedPosition);
                }
            }
            else if (BattleManager.Instance.CharacterManagement.GetCharacterInPosition(attackedPosition) != null)
            {
                Character targetCharacter = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(attackedPosition);
                attackData.Data[NotificationDataIDs.ActionTarget] = targetCharacter.ToString();

                attackData.Data[NotificationDataIDs.PreviousHP] = BattleManager.Instance.BattleValues.PartyHealth[targetCharacter.CharacterID];
                BattleManager.Instance.BattleValues.CharacterTakeDamage(targetCharacter.CharacterID, damageProvided);
                attackData.Data[NotificationDataIDs.NewHP] = BattleManager.Instance.BattleValues.PartyHealth[targetCharacter.CharacterID];

                if (PassedCritical())
                {
                    BattleManager.Instance.BattleValues.CharacterModifyWillPower(targetCharacter.CharacterID, BattleGridUtils.ReceivedCriticalWillPercentage);
                }
            }
            else if (BattleManager.Instance.BattleGrid.GetObstacleHP(attackedPosition) > 0)
            {
                attackData.Data[NotificationDataIDs.ActionTarget] = "Obstacle";
                attackData.Data[NotificationDataIDs.PreviousHP] = BattleManager.Instance.BattleGrid.GetObstacleHP(attackedPosition);
                BattleManager.Instance.BattleGrid.HitObstacle(attackedPosition, damageProvided);
                attackData.Data[NotificationDataIDs.NewHP] = BattleManager.Instance.BattleGrid.GetObstacleHP(attackedPosition);
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
        return chanceResult >= failure + success;
    }

}