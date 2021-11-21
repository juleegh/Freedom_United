using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject selectionPreview;

    public void UpdateSelection(int x, int y)
    {
        selectionPreview.transform.position = BattleGridUtils.TranslatedPosition(x, y, 0.2f);
    }

    public void Toggle(bool visible)
    {
        selectionPreview.SetActive(visible);
    }
}
