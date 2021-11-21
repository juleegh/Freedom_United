using UnityEngine;

public class CharacterMoveAction : ExecutingAction
{
    private Vector2Int finalPosition;
    private Vector2Int originPosition;
    private bool changeWithTeammate;

    public CharacterMoveAction(AllyAction scheduledAction) : base(scheduledAction)
    {
        changeWithTeammate = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(scheduledAction.position) != null;

        finalPosition = scheduledAction.position;
        originPosition = BattleManager.Instance.CharacterManagement.Characters[scheduledAction.actionOwner].CurrentPosition;
    }

    public override void Execute()
    {
        if (changeWithTeammate)
        {
            BattleManager.Instance.CharacterManagement.GetCharacterInPosition(finalPosition).MoveToPosition(originPosition);
        }

        BattleManager.Instance.CharacterManagement.GetCharacterInPosition(originPosition).MoveToPosition(finalPosition);

        GameNotificationsManager.Instance.Notify(GameNotification.CharacterMoved);
    }
}