using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionSelectionOption : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void Config(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
