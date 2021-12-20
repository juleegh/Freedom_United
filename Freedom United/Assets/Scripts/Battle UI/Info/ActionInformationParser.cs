using UnityEngine;

public class ActionInformationParser
{
    private enum TargetType
    {
        Empty,
        Character,
        BossPart,
    }

    private string Speed { get { return "Speed: " + BattleActionsUtils.GetActionSpeed(); } }
    private CharacterID ActionCharacterID { get { return BattleGridUtils.GetCharacterID(BattleUINavigation.Instance.NavigationState.currentAction.ActionOwner); } }
    private bool SelectingCell { get { return BattleUINavigation.Instance.CurrentLevel == BattleSelectionLevel.Cell; } }

    private TargetType ActionTarget
    {
        get
        {
            Character character = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(TargetPosition);
            if (character != null) return TargetType.Character;
            BossPart bossPart = BattleManager.Instance.CharacterManagement.GetBossPartInPosition(TargetPosition);
            if (bossPart != null) return TargetType.BossPart;
            return TargetType.Empty;
        }
    }

    private Vector2Int TargetPosition { get { return BattleUINavigation.Instance.NavigationState.currentAction.position; } }
    private Character TargetCharacter { get { return BattleManager.Instance.CharacterManagement.GetCharacterInPosition(TargetPosition); } }
    private BossPart TargetBossPart { get { return BattleManager.Instance.CharacterManagement.GetBossPartInPosition(TargetPosition); } }
    private Character ActionCharacter { get { return BattleManager.Instance.CharacterManagement.Characters[ActionCharacterID]; } }
    private BattleActionType ActionType { get { return BattleUINavigation.Instance.NavigationState.currentAction.actionType; } }
    private string Jump { get { return "\n"; } }

    public string DefineActionTitle()
    {
        switch (ActionType)
        {
            case BattleActionType.Attack:
                return "Attack";
            case BattleActionType.Defend:
                if (!SelectingCell)
                    return "Defend";
                else if (TargetPosition == ActionCharacter.CurrentPosition)
                    return "Personal Defense";
                else
                    return "Group Defense";
            case BattleActionType.MoveSafely:
                return "Move Safely";
            case BattleActionType.MoveFast:
                return "Move Fast";
        }

        return "Action not available";
    }

    public string DefineActionDescription()
    {
        switch (ActionType)
        {
            case BattleActionType.Attack:
                return AttackActionDescription();
            case BattleActionType.Defend:
                return DefenseActionDescription();
            case BattleActionType.MoveSafely:
                return Speed;
            case BattleActionType.MoveFast:
                return Speed + Jump + "Bump Damage: " + BattleGridUtils.ShovingDamage;
        }

        return "Action not available";
    }

    public string DefineActionTarget()
    {
        switch (ActionTarget)
        {
            case TargetType.Character:
                return CharacterTargetDescription();
            case TargetType.BossPart:
                return BossTargetDescription();
            case TargetType.Empty:
            default:
                return " -- ";
        }
    }

    private string AttackActionDescription()
    {
        string content = "";
        content += Speed + Jump;
        content += "Type of damage: " + BattleManager.Instance.PartyStats.Stats[ActionCharacterID].AttackType + Jump;
        content += "Damage: " + BattleManager.Instance.PartyStats.Stats[ActionCharacterID].BaseAttack + Jump;
        content += (BattleManager.Instance.PartyStats.Stats[ActionCharacterID].NormalSuccessChance * 100f) + "% chance of success" + Jump;
        content += (BattleManager.Instance.PartyStats.Stats[ActionCharacterID].CriticalSuccessChance * 100f) + "% chance of critical";

        return content;
    }

    private string DefenseActionDescription()
    {
        string content = "";
        content += Speed + Jump;
        if (!SelectingCell || (TargetPosition != ActionCharacter.CurrentPosition))
            content += "Base defense: " + BattleManager.Instance.PartyStats.Stats[ActionCharacterID].BaseDefense;
        else
            content += "Base defense (split): " + (BattleManager.Instance.PartyStats.Stats[ActionCharacterID].BaseDefense * BattleGridUtils.DefenseSplitFactor);

        return content;
    }

    private string CharacterTargetDescription()
    {
        string content = "";
        content += TargetCharacter.CharacterID.ToString() + Jump;
        content += "Health: " + BattleManager.Instance.BattleValues.PartyHealth[ActionCharacterID] + " / " + BattleManager.Instance.PartyStats.Stats[ActionCharacterID].BaseHealth;

        return content;
    }

    private string BossTargetDescription()
    {
        string content = "";
        content += BattleManager.Instance.CharacterManagement.BossConfig.BossName + "(" + TargetBossPart.ToString() + ")" + Jump;
        content += "Total Health: " + BattleManager.Instance.BattleValues.BossHealth + " / " + BattleManager.Instance.CharacterManagement.BossConfig.BaseHealth + Jump;
        content += "Part Health: " + BattleManager.Instance.BattleValues.BossPartsHealth[TargetBossPart.PartType] + " / " + BattleManager.Instance.CharacterManagement.BossConfig.PartsList[TargetBossPart.PartType].BaseDurability;

        return content;
    }
}