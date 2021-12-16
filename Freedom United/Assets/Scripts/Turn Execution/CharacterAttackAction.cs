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

    public CharacterAttackAction(AllyAction scheduledAction) : base(scheduledAction)
    {
        attackingCharacter = scheduledAction.actionOwner;
        attackDelta = BattleGridUtils.GetRangeConversion(BattleManager.Instance.PartyStats.Stats[scheduledAction.actionOwner].AttackRange);
        attackDirection = scheduledAction.position - BattleManager.Instance.CharacterManagement.Characters[attackingCharacter].CurrentPosition;
        attackDirection = new Vector2Int(attackDirection.x / attackDirection.x, attackDirection.y / attackDirection.y);

        attackedPositions = new List<Vector2Int>();
        for (int i = 0; i < attackDelta; i++)
        {
            attackedPositions.Add(attackDirection * i);
        }

        damageTaken = BattleManager.Instance.PartyStats.Stats[scheduledAction.actionOwner].BaseAttack;
    }

    public override void Execute()
    {
        foreach (Vector2Int position in attackedPositions)
        {
            Vector2Int attackedPosition = position + BattleManager.Instance.CharacterManagement.Characters[attackingCharacter].CurrentPosition;
            float defenseInPosition = TurnExecutor.Instance.DefenseValueInPosition(attackedPosition);
            float damageProvided = damageTaken - defenseInPosition;
            damageProvided = Mathf.Clamp(damageProvided, 0, damageProvided);

            if (BattleManager.Instance.CharacterManagement.GetBossPartInPosition(attackedPosition) != null)
            {
                BossPart targetPart = BattleManager.Instance.CharacterManagement.GetBossPartInPosition(attackedPosition);
                BattleManager.Instance.BattleValues.BossTakeDamage(targetPart.PartType, damageProvided);
                GameNotificationsManager.Instance.Notify(GameNotification.BossStatsModified);
            }
            else if (BattleManager.Instance.CharacterManagement.GetCharacterInPosition(attackedPosition) != null)
            {
                Character targetCharacter = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(attackedPosition);
                BattleManager.Instance.BattleValues.CharacterTakeDamage(targetCharacter.CharacterID, damageProvided);
            }
        }
    }
}