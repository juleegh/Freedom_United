using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DeathIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer theIcon;
    [SerializeField] private float fadeIn;

    private Color hidden = new Color(0, 0, 0, 0);
    private Color visible = new Color(1, 0, 0, 1);


    void Awake()
    {
        theIcon.color = hidden;
    }

    public void Died()
    {
        FadeIn();
    }

    private void FadeIn()
    {
        theIcon.DOColor(visible, fadeIn);
    }
}
