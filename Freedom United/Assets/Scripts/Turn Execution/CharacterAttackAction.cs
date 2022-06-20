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

        attackedPositions = GetAttackPositions();
        damageTaken = BattleManager.Instance.PartyStats.Stats[scheduledAction.actionOwner].BaseAttack;
        chanceResult = Random.Range(0f, 1f);
    }

    public List<Vector2Int> GetAttackPositions()
    {
        List<Vector2Int> attackedPositions = new List<Vector2Int>();
        for (int i = 1; i <= attackDelta; i++)
        {
            attackedPositions.Add(attackDirection * i);
        }
        return attackedPositions;
    }

    public override void Execute()
    {
        List<Vector2Int> targetPositions = BattleGridUtils.GetTranslatedPositions(BattleManager.Instance.CharacterManagement.Characters[attackingCharacter].CurrentPosition, attackedPositions);
        CameraFocus.Instance.FocusForAttack(targetPositions, critical: PassedCritical(), failed: FailedSuccess());
        foreach (Vector2Int attackedPosition in targetPositions)
        {
            GameNotificationData attackData = new GameNotificationData();
            attackData.Data[NotificationDataIDs.ActionOwner] = attackingCharacter.ToString();
            attackData.Data[NotificationDataIDs.Failure] = FailedSuccess();
            attackData.Data[NotificationDataIDs.Critical] = PassedCritical();
            attackData.Data[NotificationDataIDs.ActionTarget] = "empty";
            attackData.Data[NotificationDataIDs.NewHP] = 0f;
            attackData.Data[NotificationDataIDs.PreviousHP] = 0f;
            attackData.Data[NotificationDataIDs.CellPosition] = attackedPosition;

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
                    {
                        int wpGain = BattleManager.Instance.PartyStats.Stats[attackingCharacter].HappyWPDelta;
                        BattleManager.Instance.BattleValues.CharacterModifyWillPower(attackingCharacter, wpGain);
                        BattleManager.Instance.BattleGrid.AddPartObstacle(targetPart.PartType);
                    }
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

        if (FailedSuccess())
        {
            int wpLoss = BattleManager.Instance.PartyStats.Stats[attackingCharacter].SadWPDelta;
            BattleManager.Instance.BattleValues.CharacterModifyWillPower(attackingCharacter, wpLoss);
        }
        else if (PassedCritical())
        {
            int wpWin = BattleManager.Instance.PartyStats.Stats[attackingCharacter].HappyWPDelta;
            BattleManager.Instance.BattleValues.CharacterModifyWillPower(attackingCharacter, wpWin);
        }

        PlayAttackSound();
    }

    private bool FailedSuccess()
    {
        return chanceResult <= BattleManager.Instance.PartyStats.Stats[attackingCharacter].CriticalFailureChance;
    }

    private bool PassedCritical()
    {
        float failure = BattleManager.Instance.PartyStats.Stats[attackingCharacter].CriticalFailureChance;
        float success = BattleManager.Instance.PartyStats.Stats[attackingCharacter].NormalSuccessChance;
        return chanceResult >= failure + success;
    }

    private void PlayAttackSound()
    {
        AudioEvent attackEvent = AudioEvent.CharacterNormalAttack;

        if (FailedSuccess())
            attackEvent = AudioEvent.AttackFailed;
        else if (PassedCritical())
            attackEvent = AudioEvent.CriticalAttack;

        GameAudio.Instance.AudioToEvent(attackEvent);
    }
}