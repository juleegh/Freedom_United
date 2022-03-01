using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Attack Range Around Killed Body Part")]
public class AIAttackRangeAroundBodyPart : AIAttackAction
{
    [SerializeField] private int range = 2;

    protected override void AddToActionPile()
    {
        bool result = SelectRangeAroundDeathPart();
        if (!result)
            AddToPileDefault();
    }

    private bool SelectRangeAroundDeathPart()
    {
        List<Vector2Int> rangeAround = new List<Vector2Int>();

        List<Vector2Int> partPositions = GetDeathPartPositions();
        List<Vector2Int> attackRange = GetRangeAroundPositions(range, partPositions);

        foreach (Vector2Int positionToAttack in attackRange)
        {
            Character characterInPosition = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(positionToAttack);
            if (characterInPosition == null || !BattleManager.Instance.BattleValues.IsAlive(characterInPosition.CharacterID))
                continue;

            BossPart attackingPart = BossUtils.GetPartWhoCanAttackPosition(positionToAttack);
            SetOfPositions usedAreaOfEffect = null;

            if (attackingPart != null)
            {
                usedAreaOfEffect = BossUtils.GetAreaOfEffectForPosition(attackingPart, positionToAttack);
                AddAttackActionToPile(attackingPart, usedAreaOfEffect);
                return true;
            }
        }

        return false;
    }

    private List<Vector2Int> GetDeathPartPositions()
    {
        List<Vector2Int> partPositions = new List<Vector2Int>();

        foreach (PostActionInfo actionInfo in TurnBlackBoard.Instance.LastTurnRegisters)
        {
            if (BattleActionsUtils.GetTargetType(actionInfo.actionTarget) == TargetType.BossPart)
            {
                if (actionInfo.previousTargetHP > 0f && actionInfo.newTargetHP <= 0f)
                {
                    BossPartType brokenPart = BattleGridUtils.GetBossPart(actionInfo.actionTarget);
                    BossPartConfig partConfig = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[brokenPart];
                    partPositions = BossUtils.GetPositionsOccupiedByPart(partConfig.PartType);
                }
            }
        }

        return partPositions;
    }

    private List<Vector2Int> GetRangeAroundPositions(int range, List<Vector2Int> partPositions)
    {
        List<Vector2Int> rangeAround = new List<Vector2Int>();

        for (int column = -range; column <= range; column++)
        {
            for (int row = -range; row <= range; row++)
            {
                if (column < 0 || row < 0)
                    continue;

                if (column >= BattleManager.Instance.BattleGrid.Width)
                    continue;
                if (row >= BattleManager.Instance.BattleGrid.Height)
                    continue;

                Vector2Int temptingPosition = new Vector2Int(column, row);

                if (partPositions.Contains(temptingPosition))
                    continue;

                rangeAround.Add(temptingPosition);
            }
        }

        return rangeAround;
    }
}
