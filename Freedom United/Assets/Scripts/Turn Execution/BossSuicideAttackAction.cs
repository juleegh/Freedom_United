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
        CameraFocus.Instance.FocusForAttack(selectedCells, critical: true, failed: false);
        foreach (Vector2Int attackedPosition in selectedCells)
        {
            float damageProvided = damageTaken;

            List<Vector2Int> defendingCharacters = TurnExecutor.Instance.GetDefendingPositions(attackedPosition);
            GameNotificationData defenseData = new GameNotificationData();

            foreach (Vector2Int defender in defendingCharacters)
            {
                float damageForDefense = (float) damageProvided / (float) defendingCharacters.Count;
                float defenseProvided = 0;

                Character defenderCharacter = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(defender);
                if (defenderCharacter != null)
                {
                    defenseProvided = BattleManager.Instance.BattleValues.PartyDefense[defenderCharacter.CharacterID];
                }
                else
                {
                    defenseProvided = BattleManager.Instance.BattleGrid.GetObstacleHP(defender);
                }

                if (defenseProvided < damageForDefense)
                {
                    damageForDefense = defenseProvided;
                }
                damageProvided -= damageForDefense;
                if (damageProvided <= 0)
                    damageProvided = 0;

                if (defenderCharacter != null)
                {
                    BattleManager.Instance.BattleValues.CharacterModifyDefensePower(defenderCharacter.CharacterID, damageForDefense);
                    defenseData.Data[NotificationDataIDs.ActionOwner] = defenderCharacter.CharacterID;
                    defenseData.Data[NotificationDataIDs.CellPosition] = BattleManager.Instance.CharacterManagement.Characters[defenderCharacter.CharacterID].CurrentPosition;
                    GameNotificationsManager.Instance.Notify(GameNotification.DefenseWasHit, defenseData);

                    if (BattleManager.Instance.BattleValues.PartyDefense[defenderCharacter.CharacterID] <= 0)
                    {
                        foreach (Vector2Int defenderPos in TurnExecutor.Instance.GetDefendedPositions(defenderCharacter.CharacterID))
                        {
                            GameNotificationData defenseBrokenData = new GameNotificationData();
                            defenseBrokenData.Data[NotificationDataIDs.CellPosition] = defenderPos;
                            defenseBrokenData.Data[NotificationDataIDs.ShieldState] = TurnExecutor.Instance.DefenseValueInPosition(defenderPos) > 0;
                            GameNotificationsManager.Instance.Notify(GameNotification.DefenseWasUpdated, defenseBrokenData);
                        }
                    }
                }
                else
                {
                    BattleManager.Instance.BattleGrid.HitObstacle(defender, damageForDefense);
                    
                    if (BattleManager.Instance.BattleGrid.GetObstacleHP(defender) <= 0)
                    {
                        GameNotificationData defenseBrokenData = new GameNotificationData();
                        defenseBrokenData.Data[NotificationDataIDs.CellPosition] = attackedPosition;
                        defenseBrokenData.Data[NotificationDataIDs.ShieldState] = TurnExecutor.Instance.DefenseValueInPosition(attackedPosition) > 0;
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
        GameAudio.Instance.AudioToEvent(AudioEvent.BossSuicideAttack);
    }
}