using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossAttackAction : ExecutingAction
{
    private BossPartType attackingPart;
    private Vector2Int pivot;
    private SetOfPositions attackShape;

    private float damageTaken;

    private float chanceResult;

    public BossAttackAction(BossAction scheduledAction) : base(scheduledAction)
    {
        attackingPart = scheduledAction.actionOwner;
        damageTaken = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[attackingPart].BaseAttack;
        attackShape = scheduledAction.deltaOfAction;
        pivot = scheduledAction.position;
        chanceResult = Random.Range(0f, 1f);
    }

    public override void Execute()
    {
        Vector2Int currentPartPosition = BattleManager.Instance.CharacterManagement.Boss.Parts[attackingPart].Position;
        Vector2Int currentPartOrientation = BattleManager.Instance.CharacterManagement.Boss.Parts[attackingPart].Orientation;
        List<Vector2Int> attackedPositions = attackShape.GetRotatedDeltasWithPivot(pivot, currentPartOrientation);

        foreach (Vector2Int attackPosition in attackedPositions)
        {
            Vector2Int attackedPosition = attackPosition + currentPartPosition;
            float damageProvided = damageTaken;
            if (FailedSuccess())
                damageProvided = 0;
            else if (PassedCritical())
                damageProvided *= BattleGridUtils.BossCriticalDamageMultiplier;

            List<CharacterID> defendingCharacters = TurnExecutor.Instance.GetDefendingCharacters(attackedPosition);
            GameNotificationData defenseData = new GameNotificationData();

            foreach (CharacterID defender in defendingCharacters)
            {
                float damageForDefense = (float) damageProvided / (float) defendingCharacters.Count;
                float defenseProvided = BattleManager.Instance.BattleValues.PartyDefense[defender];
                if (defenseProvided < damageForDefense)
                {
                    damageForDefense = defenseProvided;
                }
                damageProvided -= damageForDefense;
                if (damageProvided <= 0)
                    damageProvided = 0;

                BattleManager.Instance.BattleValues.CharacterModifyDefensePower(defender, damageForDefense);
                defenseData.Data[NotificationDataIDs.ActionOwner] = defender;
                defenseData.Data[NotificationDataIDs.CellPosition] = BattleManager.Instance.CharacterManagement.Characters[defender].CurrentPosition;
                GameNotificationsManager.Instance.Notify(GameNotification.DefenseWasHit, defenseData);

                if (BattleManager.Instance.BattleValues.PartyDefense[defender] <= 0)
                {
                    foreach (Vector2Int defenderPos in TurnExecutor.Instance.GetDefendedPositions(defender))
                    {
                        GameNotificationData defenseBrokenData = new GameNotificationData();
                        defenseBrokenData.Data[NotificationDataIDs.CellPosition] = defenderPos;
                        defenseBrokenData.Data[NotificationDataIDs.ShieldState] = TurnExecutor.Instance.DefenseValueInPosition(defenderPos) > 0;
                        GameNotificationsManager.Instance.Notify(GameNotification.DefenseWasUpdated, defenseBrokenData);
                    }
                }
            }

            //if (damageProvided <= 0)
                //continue;

            GameNotificationData attackData = new GameNotificationData();
            attackData.Data[NotificationDataIDs.ActionOwner] = attackingPart.ToString();
            attackData.Data[NotificationDataIDs.Failure] = FailedSuccess();
            attackData.Data[NotificationDataIDs.Critical] = PassedCritical();
            attackData.Data[NotificationDataIDs.ActionTarget] = "empty";
            attackData.Data[NotificationDataIDs.NewHP] = 0f;
            attackData.Data[NotificationDataIDs.PreviousHP] = 0f;
            attackData.Data[NotificationDataIDs.CellPosition] = attackedPosition;

            if (BattleManager.Instance.CharacterManagement.GetCharacterInPosition(attackedPosition) != null)
            {
                Character targetCharacter = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(attackedPosition);
                attackData.Data[NotificationDataIDs.ActionTarget] = targetCharacter.ToString();

                attackData.Data[NotificationDataIDs.PreviousHP] = BattleManager.Instance.BattleValues.PartyHealth[targetCharacter.CharacterID];
                BattleManager.Instance.BattleValues.CharacterTakeDamage(targetCharacter.CharacterID, damageProvided);
                attackData.Data[NotificationDataIDs.NewHP] = BattleManager.Instance.BattleValues.PartyHealth[targetCharacter.CharacterID];

                if (PassedCritical())
                {
                    int wpLoss = BattleManager.Instance.PartyStats.Stats[targetCharacter.CharacterID].SadWPDelta;
                    BattleManager.Instance.BattleValues.CharacterModifyWillPower(targetCharacter.CharacterID, wpLoss);
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