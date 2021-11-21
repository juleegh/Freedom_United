public class CharacterAttackAction : ExecutingAction
{
    private BossPart targetPart;
    private int damageTaken;

    public CharacterAttackAction(AllyAction scheduledAction) : base(scheduledAction)
    {
        targetPart = BattleManager.Instance.CharacterManagement.GetBossPartInPosition(scheduledAction.position);

        damageTaken = BattleManager.Instance.PartyStats.Stats[scheduledAction.actionOwner].BaseAttack;
    }

    public override void Execute()
    {
        BattleManager.Instance.BattleValues.BossTakeDamage(targetPart.PartType, damageTaken);
        GameNotificationsManager.Instance.Notify(GameNotification.BossStatsModified);
    }
}