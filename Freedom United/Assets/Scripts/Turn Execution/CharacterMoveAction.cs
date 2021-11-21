public class CharacterMoveAction : ExecutingAction
{
    public CharacterMoveAction(AllyAction scheduledAction) : base(scheduledAction)
    {
        representedAction = scheduledAction;
    }
}