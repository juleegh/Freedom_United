public class PostActionInfo
{
    public string actionOwner;
    public string actionTarget;

    public BattleActionType actionType;
    public bool wasFailure;
    public bool wasCritical;

    public float previousTargetHP;
    public float newTargetHP;
}