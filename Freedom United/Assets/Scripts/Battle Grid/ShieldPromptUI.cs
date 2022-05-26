using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ShieldPromptUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer shieldIcon;
    [SerializeField] private float fadeIn;
    [SerializeField] private float fadeOut;
    [SerializeField] private Color visible;

    private Color hidden = new Color(0, 0, 0, 0);

    private bool isGuarding;

    void Awake()
    {
        Clear();
    }

    public void Clear()
    {
        shieldIcon.color = hidden;
        isGuarding = false;
    }

    public void ShowShield(bool guarded)
    {
        if (isGuarding && !guarded)
        {
            isGuarding = false;
            FadeOut();
        }
        else if (!isGuarding && guarded)
        {
            isGuarding = true;
            FadeIn();
        }
    }

    private void FadeIn()
    {
        shieldIcon.DOColor(visible, fadeIn);
    }

    private void FadeOut()
    {
        shieldIcon.DOColor(hidden, fadeOut);
    }
}
