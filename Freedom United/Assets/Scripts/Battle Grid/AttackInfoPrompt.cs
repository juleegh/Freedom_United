using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class AttackInfoPrompt : MonoBehaviour
{
    [SerializeField] private TextMeshPro textPrompt;
    [SerializeField] private float fadeIn;
    [SerializeField] private float fadeOut;
    [SerializeField] private float delay;

    private Color hidden = new Color(0, 0, 0, 0);
    private Color visible = new Color(1, 0, 0, 1);
    private Sequence sequence;

    void Awake()
    {
        Clear();
        textPrompt.gameObject.SetActive(true);
    }

    public void Clear()
    {
        textPrompt.color = hidden;
        textPrompt.text = "";
    }

    public void ShowCritical()
    {
        textPrompt.text = "CRITICAL";
        RunPrompt();
    }

    public void ShowFailure()
    {
        textPrompt.text = "FAILED";
        RunPrompt();
    }

    private void RunPrompt()
    {
        textPrompt.color = hidden;
        DOTween.Kill(sequence);
        FadeIn();

        sequence = DOTween.Sequence();
        sequence.AppendInterval(delay);
        sequence.OnComplete(FadeOut);
        sequence.Play();
    }

    private void FadeIn()
    {
        textPrompt.DOColor(visible, fadeIn);
    }

    private void FadeOut()
    {
        textPrompt.DOColor(hidden, fadeOut);
    }

}
