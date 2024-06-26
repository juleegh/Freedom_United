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
        GameNotificationData notificationData = new GameNotificationData();
        originPosition = BattleManager.Instance.CharacterManagement.Characters[movingCharacter].CurrentPosition;
        changeWithTeammate = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(finalPosition) != null;

        if (changeWithTeammate)
        {
            Character targetCharacter = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(finalPosition);

            if (!isSafe)
            {
                BattleManager.Instance.BattleValues.CharacterTakeDamage(targetCharacter.CharacterID, BattleGridUtils.ShovingDamage);
            }

            targetCharacter.MoveToPosition(originPosition);
            notificationData.Data[NotificationDataIDs.ActionOwner] = targetCharacter.CharacterID;
            notificationData.Data[NotificationDataIDs.CellPosition] = originPosition;
            notificationData.Data[NotificationDataIDs.WasReckless] = !isSafe;
            notificationData.Data[NotificationDataIDs.WasPushed] = !isSafe;
            GameNotificationsManager.Instance.Notify(GameNotification.CharacterMoved, notificationData);
        }

        BattleManager.Instance.CharacterManagement.Characters[movingCharacter].MoveToPosition(finalPosition);

        notificationData.Data[NotificationDataIDs.ActionOwner] = movingCharacter;
        notificationData.Data[NotificationDataIDs.CellPosition] = finalPosition;
        notificationData.Data[NotificationDataIDs.WasReckless] = !isSafe;
        notificationData.Data[NotificationDataIDs.WasPushed] = false;
        GameNotificationsManager.Instance.Notify(GameNotification.CharacterMoved, notificationData);
        PlayMoveSound();
    }

    private void PlayMoveSound()
    {
        if (isSafe)
            GameAudio.Instance.AudioToEvent(AudioEvent.MovedCarefully);
        else
            GameAudio.Instance.AudioToEvent(AudioEvent.MovedFast);
    }
}