public static class ActionParser
{
    public static ExecutingAction GetParsedAction(ScheduledAction scheduledAction)
    {
        if (BattleGridUtils.IsACharacter(scheduledAction.ActionOwner))
        {
            AllyAction allyAction = scheduledAction as AllyAction;
            switch (allyAction.actionType)
            {
                case BattleActionType.Attack:
                    return new CharacterAttackAction(allyAction);
                case BattleActionType.Defend:
                    return new CharacterDefenseAction(allyAction);
                case BattleActionType.MoveSafely:
                    return new CharacterMoveAction(allyAction, true);
                case BattleActionType.MoveFast:
                    return new CharacterMoveAction(allyAction, false);
            }
        }
        else
        {
            BossAction bossAction = scheduledAction as BossAction;
            switch (bossAction.actionType)
            {
                case BattleActionType.Attack:
                    return new BossAttackAction(bossAction);
                case BattleActionType.Defend:
                    return new BossDefenseAction(bossAction);
                case BattleActionType.Rotate:
                    return new BossRotateAction(bossAction);
                case BattleActionType.SuicideAttack:
                    return new BossSuicideAttackAction(bossAction);
            }
        }

        return null;
    }
}