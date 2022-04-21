using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePromptUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro damageTaken;
    [SerializeField] private SpriteRenderer damageIcon;
    [SerializeField] private float fadeIn;
    [SerializeField] private float fadeOut;
    [SerializeField] private float delay;

    private Color hidden = new Color(0, 0, 0, 0);
    private Color visible = new Color(1, 0, 0, 1);
    private Sequence sequence;

    void Awake()
    {
        Clear();
        damageTaken.gameObject.SetActive(true);
    }

    public void Clear()
    {
        damageTaken.color = hidden;
        damageIcon.color = hidden;
    }

    public void ShowDamage(float taken)
    {
        damageTaken.color = hidden;
        damageIcon.color = hidden;
        damageTaken.text = "-" + taken;
        DOTween.Kill(sequence);
        FadeIn();

        sequence = DOTween.Sequence();
        sequence.AppendInterval(delay);
        sequence.OnComplete(FadeOut);
        sequence.Play();
    }

    private void FadeIn()
    {
        damageTaken.DOColor(visible, fadeIn);
        damageIcon.DOColor(visible, fadeIn);
    }

    private void FadeOut()
    {
        damageTaken.DOColor(hidden, fadeOut);
        damageIcon.DOColor(hidden, fadeOut);
    }
}
