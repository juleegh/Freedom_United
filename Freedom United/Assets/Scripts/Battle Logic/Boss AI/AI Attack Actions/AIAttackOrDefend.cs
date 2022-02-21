using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AI Attack Or Defend")]
public class AIAttackOrDefend : AIAttackAction
{
    [SerializeField] private float defendProbability = 0.5f;

    protected override void AddToActionPile()
    {
        // At random chooses to attack any random character or any defense action 
        if (WillDefend())
        {
            BossPartConfig defendingPart = SelectRandomPartToDefend();
            SetOfPositions defenseArea = GetDefendArea(defendingPart);
            AddDefenseActionToPile(BattleManager.Instance.CharacterManagement.Boss.Parts[defendingPart.PartType], defenseArea);
        }
        else
        {
            bool result = SelectRandomTarget();
            if (!result)
                SelectCounterPartTarget();
        }
    }

    private bool WillDefend()
    {
        return Random.Range(0f, 1f) <= defendProbability;
    }

    private BossPartConfig SelectRandomPartToDefend()
    {
        List<BossPartConfig> parts = new List<BossPartConfig>(BattleManager.Instance.CharacterManagement.BossConfig.PartsList.Values);

        while (parts.Count > 0)
        {
            int randomPosition = Random.Range(0, parts.Count);
            BossPartConfig randomPart = parts[randomPosition];
            parts.Remove(randomPart);

            if (randomPart.BossDefenseType != BossDefenseType.None)
                return randomPart;
        }

        return null;
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
