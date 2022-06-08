using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject selectionPreview;
    [SerializeField] private StatsPreview statsPreview;

    public void UpdateSelection(int x, int y)
    {
        selectionPreview.transform.position = BattleGridUtils.TranslatedPosition(x, y, 0.1f);
        UpdateHPPreview(x, y);
    }

    public void Toggle(bool visible)
    {
        selectionPreview.SetActive(visible);
    }

    private void UpdateHPPreview(int x, int y)
    {
        Vector2Int position = new Vector2Int(x, y);
        float hp = BattleManager.Instance.BattleGrid.GetObstacleHP(position);
        Character character = BattleManager.Instance.CharacterManagement.GetCharacterInPosition(position);
        BossPart bossPart = BattleManager.Instance.CharacterManagement.GetBossPartInPosition(position);

        if (character != null) 
            hp = BattleManager.Instance.BattleValues.PartyHealth[character.CharacterID];
        else if(bossPart != null) 
            hp = BattleManager.Instance.BattleValues.BossPartsHealth[bossPart.PartType];

        statsPreview.Toggle(hp > 0);
        statsPreview.SetHPValue(hp);

    }
}
