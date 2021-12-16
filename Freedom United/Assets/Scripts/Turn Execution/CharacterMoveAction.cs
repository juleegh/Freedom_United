using UnityEngine;

public class CharacterMoveAction : ExecutingAction
{
    private CharacterID movingCharacter;
    private Vector2Int finalPosition;
    private Vector2Int originPosition;
    private bool changeWithTeammate;

    public CharacterMoveAction(AllyAction scheduledAction) : base(scheduledAction)
    {
        finalPosition = scheduledAction.position;
        movingCharacter = scheduledAction.actionOwner;
    }

    public override void Execute()
    {
        originPosition = BattleManager.Instance.CharacterManagement.Characters[movingCharacter].CurrentPosition;
        changeWithTeammate = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(finalPosition) != null;

        if (changeWithTeammate)
            BattleManager.Instance.CharacterManagement.GetCharacterInPosition(finalPosition).MoveToPosition(originPosition);

        BattleManager.Instance.CharacterManagement.GetCharacterInPosition(originPosition).MoveToPosition(finalPosition);

        GameNotificationsManager.Instance.Notify(GameNotification.CharacterMoved);
    }
}