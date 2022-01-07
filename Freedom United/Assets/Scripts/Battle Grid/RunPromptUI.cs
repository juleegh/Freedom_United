using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class RunPromptUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer runIcon;
    [SerializeField] private float fadeIn;
    [SerializeField] private float fadeOut;
    [SerializeField] private float delay;

    private Color hidden = new Color(0, 0, 0, 0);
    private Color visible = new Color(0, 1, 0, 1);
    private Sequence sequence;

    void Awake()
    {
        runIcon.color = hidden;
    }

    public void ShowRun()
    {
        runIcon.color = hidden;
        DOTween.Kill(sequence);
        FadeIn();

        sequence = DOTween.Sequence();
        sequence.AppendInterval(delay);
        sequence.OnComplete(FadeOut);
        sequence.Play();
    }

    private void FadeIn()
    {
        runIcon.DOColor(visible, fadeIn);
    }

    private void FadeOut()
    {
        runIcon.DOColor(hidden, fadeOut);
    }
}
