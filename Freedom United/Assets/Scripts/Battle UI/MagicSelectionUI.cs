using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSelectionUI : MonoBehaviour
{
    [SerializeField] private int spellsOnScreen;
    [SerializeField] private Transform spellsContainer;
    [SerializeField] private GameObject spellPreviewPrefab;
    [SerializeField] private GameObject UpIndicator;
    [SerializeField] private GameObject DownIndicator;
    private MagicActionOption[] spellPreviews;

    void Start()
    {
        LoadUI();
    }

    private void LoadUI()
    {
        spellPreviews = new MagicActionOption[spellsOnScreen];
        List<MagicSpell> spells = new List<MagicSpell>();

        for (int i = 0; i < spellsOnScreen; i++)
        {
            spellPreviews[i] = Instantiate(spellPreviewPrefab).GetComponent<MagicActionOption>();
            spellPreviews[i].transform.SetParent(spellsContainer);
            spellPreviews[i].gameObject.SetActive(false);
        }
        RefreshView(0, 0);
    }

    public void RefreshView(int topSpell, int selectedSpell)
    {
        int previewIndex = 0;

        List<MagicSpell> spells = BattleManager.Instance.MagicManagement.Spells;
        // TODO: Connect this eventually to actual magic

        for (int i = topSpell; i < topSpell + spellsOnScreen; i++)
        {
            if (i >= spells.Count)
            {
                spellPreviews[previewIndex].gameObject.SetActive(false);
                previewIndex++;
                continue;
            }

            spellPreviews[previewIndex].gameObject.SetActive(true);
            spellPreviews[previewIndex].Config(spells[i].spellName);
            previewIndex++;
        }

        UpIndicator.SetActive(topSpell > 0);
        DownIndicator.SetActive(topSpell + spellsOnScreen <= spells.Count - 1);
    }

    public void RefreshSelectionView(int selectedTempo)
    {
        for (int i = 0; i < spellsOnScreen; i++)
        {
            spellPreviews[i].ToggleSelected(i == selectedTempo);
        }
    }
}
