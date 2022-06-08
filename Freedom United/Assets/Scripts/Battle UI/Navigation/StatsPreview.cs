using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsPreview : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI hpText;

    public void Toggle(bool visible)
    {
        container.SetActive(visible);
    }

    public void SetHPValue(float value)
    {
        hpText.text = "HP: " + value.ToString();
    }
}
