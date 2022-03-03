using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Defend Part In Danger")]
public class AIDefendPartInDanger : AITurnAction
{
    [SerializeField] private float inDangerHP;
    [SerializeField] private int distanceLimit;

    protected override void AddToActionPile()
    {
        bool result = SelectPartToDefend();
    }

    private bool SelectPartToDefend()
    {
        List<BossPartConfig> parts = new List<BossPartConfig>(BattleManager.Instance.CharacterManagement.BossConfig.PartsList.Values);
        BossPartType partToDefend = BossPartType.RobotHead;
        bool shouldDefend = false;

        foreach (BossPartConfig part in parts)
        {
            if (BattleManager.Instance.BattleValues.BossPartsHealth[part.PartType] <= inDangerHP && CanBeDefended(part.PartType))
            {
                BossPart bossPart = BattleManager.Instance.CharacterManagement.Boss.Parts[part.PartType];
                foreach (Character character in BattleManager.Instance.CharacterManagement.Characters.Values)
                {
                    Vector2Int up = character.CurrentPosition + Vector2Int.up * distanceLimit;
                    Vector2Int down = character.CurrentPosition + Vector2Int.down * distanceLimit;
                    Vector2Int left = character.CurrentPosition + Vector2Int.left * distanceLimit;
                    Vector2Int right = character.CurrentPosition + Vector2Int.right * distanceLimit;

                    if (bossPart.OccupiesPosition(up.x, up.y) || bossPart.OccupiesPosition(down.x, down.y) ||
                        bossPart.OccupiesPosition(left.x, left.y) || bossPart.OccupiesPosition(right.x, right.y))
                    {
                        partToDefend = part.PartType;
                        shouldDefend = true;
                        break;
                    }
                }
            }
        }

        if (shouldDefend)
        {
            foreach (BossPartConfig part in parts)
            {
                if (part.DefendedParts.Contains(partToDefend))
                {
                    SetOfPositions defenseArea = GetDefendArea(part);
                    AddDefenseActionToPile(BattleManager.Instance.CharacterManagement.Boss.Parts[part.PartType], defenseArea);
                    return true;
                }
            }
        }

        return false;
    }

    private bool CanBeDefended(BossPartType toDefend)
    {
        List<BossPartConfig> parts = new List<BossPartConfig>(BattleManager.Instance.CharacterManagement.BossConfig.PartsList.Values);

        foreach (BossPartConfig part in parts)
        {
            if (part.DefendedParts.Contains(toDefend))
            {
                return true;
            }
        }
        return false;
    }

    private SetOfPositions GetDefendArea(BossPartConfig defendingPart)
    {
        List<Vector2Int> positionsToDefend = new List<Vector2Int>();

        foreach (BossPartType defendedPart in defendingPart.DefendedParts)
        {
            BossPartConfig partConfig = BattleManager.Instance.CharacterManagement.BossConfig.PartsList[defendedPart];
            List<Vector2Int> partPositions = BossUtils.GetPositionsOccupiedByPart(partConfig.PartType);
            positionsToDefend.AddRange(partPositions);
        }

        SetOfPositions areaOfDefense = new SetOfPositions(positionsToDefend);
        return areaOfDefense;
    }
}
