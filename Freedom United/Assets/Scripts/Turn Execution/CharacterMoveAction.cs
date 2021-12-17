using UnityEngine;

public class CharacterMoveAction : ExecutingAction
{
    private CharacterID movingCharacter;
    private Vector2Int finalPosition;
    private Vector2Int originPosition;
    private bool changeWithTeammate;
    private bool isSafe;

    public CharacterMoveAction(AllyAction scheduledAction, bool safe) : base(scheduledAction)
    {
        finalPosition = scheduledAction.position;
        movingCharacter = scheduledAction.actionOwner;
        isSafe = safe;
    }

    public override void Execute()
    {
        originPosition = BattleManager.Instance.CharacterManagement.Characters[movingCharacter].CurrentPosition;
        changeWithTeammate = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(finalPosition) != null;

        if (changeWithTeammate)
        {
            BattleManager.Instance.CharacterManagement.GetCharacterInPosition(finalPosition).MoveToPosition(originPosition);
            if (!isSafe)
            {
                Character targetCharacter = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(finalPosition);
                BattleManager.Instance.BattleValues.CharacterTakeDamage(targetCharacter.CharacterID, BattleGridUtils.ShovingDamage);
            }
        }

        BattleManager.Instance.CharacterManagement.Characters[movingCharacter].MoveToPosition(finalPosition);

        GameNotificationsManager.Instance.Notify(GameNotification.CharacterMoved);
    }
}