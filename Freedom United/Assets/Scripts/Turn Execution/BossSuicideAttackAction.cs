using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossSuicideAttackAction : ExecutingAction
{
    private List<Vector2Int> selectedCells;
    private float damageTaken;
    public BossSuicideAttackAction(BossAction scheduledAction) : base(scheduledAction)
    {
        selectedCells = scheduledAction.deltaOfAction.Positions;
        damageTaken = BattleManager.Instance.CharacterManagement.BossConfig.SuicideAttackDamage;
    }

    public override void Execute()
    {
        foreach (Vector2Int attackedPosition in selectedCells)
        {
            float damageProvided = damageTaken;

            List<CharacterID> defendingCharacters = TurnExecutor.Instance.GetDefendingCharacters(attackedPosition);
            GameNotificationData defenseData = new GameNotificationData();

            foreach (CharacterID defender in defendingCharacters)
            {
                float damageForDefense = (float)damageProvided / (float)defendingCharacters.Count;
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

            GameNotificationData attackData = new GameNotificationData();

            attackData.Data[NotificationDataIDs.ActionOwner] = BattleManager.Instance.CharacterManagement.Boss.Core.ToString();
            attackData.Data[NotificationDataIDs.Failure] = false;
            attackData.Data[NotificationDataIDs.Critical] = false;
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
}