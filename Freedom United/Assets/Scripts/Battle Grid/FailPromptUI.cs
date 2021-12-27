using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FailPromptUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro failPrompt;
    [SerializeField] private float fadeIn;
    [SerializeField] private float fadeOut;
    [SerializeField] private float delay;

    private Color hidden = new Color(0, 0, 0, 0);
    private Color visible = new Color(1, 0, 0, 1);
    private Sequence sequence;

    void Awake()
    {
        failPrompt.color = hidden;
    }

    public void ShowFailure()
    {
        failPrompt.color = hidden;
        DOTween.Kill(sequence);
        FadeIn();

        sequence = DOTween.Sequence();
        sequence.AppendInterval(delay);
        sequence.OnComplete(FadeOut);
        sequence.Play();
    }

    private void FadeIn()
    {
        failPrompt.DOColor(visible, fadeIn);
    }

    private void FadeOut()
    {
        failPrompt.DOColor(hidden, fadeOut);
    }

}
