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

    public List<Vector2Int> GetTargetPositions()
    {
        Vector2Int currentPartPosition = BattleManager.Instance.CharacterManagement.Boss.Parts[attackingPart].Position;
        Vector2Int currentPartOrientation = BattleManager.Instance.CharacterManagement.Boss.Parts[attackingPart].Orientation;
        List<Vector2Int> attackedPositions = attackShape.GetRotatedDeltasWithPivot(pivot, currentPartOrientation);

        List<Vector2Int> targetPositions = BattleGridUtils.GetTranslatedPositions(currentPartPosition, attackedPositions);
        return targetPositions;
    }

    public override void Execute()
    {
        List<Vector2Int> targetPositions = GetTargetPositions();
        CameraFocus.Instance.FocusForAttack(targetPositions, critical : PassedCritical(), failed : FailedSuccess());

        foreach (Vector2Int attackedPosition in targetPositions)
        {
            float damageProvided = damageTaken;
            if (FailedSuccess())
                damageProvided = 0;
            else if (PassedCritical())
                damageProvided *= BattleGridUtils.BossCriticalDamageMultiplier;

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
        PlayAttackSound();
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

    private void PlayAttackSound()
    {
        AudioEvent attackEvent = AudioEvent.BossRegularAttack;

        if (BattleManager.Instance.CharacterManagement.BossConfig.PartsList[attackingPart].IsCore)
            attackEvent = AudioEvent.MainBodyAttack;
        
        if (FailedSuccess())
            attackEvent = AudioEvent.AttackFailed;
        else if(PassedCritical())
            attackEvent = AudioEvent.CriticalAttack;

        GameAudio.Instance.AudioToEvent(attackEvent);
    }
}