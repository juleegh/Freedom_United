public class CharacterDefenseAction : ExecutingAction
{
    private CharacterID targetCharacter;
    private float defenseProvided;

    public CharacterDefenseAction(AllyAction scheduledAction) : base(scheduledAction)
    {
        targetCharacter = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(scheduledAction.position).CharacterID;

        defenseProvided = BattleManager.Instance.PartyStats.Stats[scheduledAction.actionOwner].BaseDefense;
    }

    public override void Execute()
    {
        //BattleManager.Instance.BattleValues.BossTakeDamage(targetPart.PartType, damageTaken);
        //GameNotificationsManager.Instance.Notify(GameNotification.BossStatsModified);
    }
}