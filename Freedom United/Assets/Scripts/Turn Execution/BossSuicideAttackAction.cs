using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossSuicideAttackAction : ExecutingAction
{
    public BossSuicideAttackAction(BossAction scheduledAction) : base(scheduledAction)
    {

    }

    public override void Execute()
    {
        int amountOfCells = BattleManager.Instance.CharacterManagement.BossConfig.AmountOfSuicideCells;
        float damageTaken = BattleManager.Instance.CharacterManagement.BossConfig.SuicideAttackDamage;

        List<Vector2Int> selectedCells = new List<Vector2Int>();
        List<Vector2Int> availableCells = new List<Vector2Int>();
        availableCells.AddRange(BattleManager.Instance.BattleGrid.GridPositions);

        while (availableCells.Count > 0 && selectedCells.Count < amountOfCells)
        {
            Vector2Int randomCell = availableCells[Random.Range(0, availableCells.Count)];
            availableCells.Remove(randomCell);

            if (BattleManager.Instance.CharacterManagement.GetBossPartInPosition(randomCell) != null)
                continue;

            selectedCells.Add(randomCell);
        }

        foreach (Vector2Int attackedPosition in selectedCells)
        {
            GameNotificationData attackData = new GameNotificationData();

            attackData.Data[NotificationDataIDs.ActionOwner] = BattleManager.Instance.CharacterManagement.Boss.Core.ToString();
            attackData.Data[NotificationDataIDs.Failure] = false;
            attackData.Data[NotificationDataIDs.Critical] = false;
            attackData.Data[NotificationDataIDs.ActionTarget] = "empty";
            attackData.Data[NotificationDataIDs.NewHP] = 0f;
            attackData.Data[NotificationDataIDs.PreviousHP] = 0f;
            attackData.Data[NotificationDataIDs.CellPosition] = attackedPosition;

            float defenseInPosition = TurnExecutor.Instance.DefenseValueInPosition(attackedPosition);
            float damageProvided = damageTaken - defenseInPosition;
            damageProvided = Mathf.Clamp(damageProvided, 0, damageProvided);

            if (BattleManager.Instance.CharacterManagement.GetCharacterInPosition(attackedPosition) != null)
            {
                Character targetCharacter = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(attackedPosition);
                attackData.Data[NotificationDataIDs.ActionTarget] = targetCharacter.ToString();

                attackData.Data[NotificationDataIDs.PreviousHP] = BattleManager.Instance.BattleValues.PartyHealth[targetCharacter.CharacterID];
                BattleManager.Instance.BattleValues.CharacterTakeDamage(targetCharacter.CharacterID, damageProvided);
                attackData.Data[NotificationDataIDs.NewHP] = BattleManager.Instance.BattleValues.PartyHealth[targetCharacter.CharacterID];

                if (defenseInPosition > 0)
                {
                    List<CharacterID> defendingCharacters = TurnExecutor.Instance.GetDefendingCharacters(attackedPosition);

                    GameNotificationData defenseData = new GameNotificationData();
                    foreach (CharacterID character in defendingCharacters)
                    {
                        BattleManager.Instance.BattleValues.CharacterModifyDefensePower(character, defenseInPosition);

                        defenseData.Data[NotificationDataIDs.ActionOwner] = character;
                        defenseData.Data[NotificationDataIDs.CellPosition] = BattleManager.Instance.CharacterManagement.Characters[character].CurrentOrientation;
                        GameNotificationsManager.Instance.Notify(GameNotification.DefenseWasHit, defenseData);
                    }
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
}