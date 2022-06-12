using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterVisuals : MonoBehaviour
{
    [SerializeField] private CharacterID characterID;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private CharacterAssets assets;
    [SerializeField] private CharacterStatsVisuals stats;
    [SerializeField] private DeathIcon deathIcon;

    public CharacterID CharacterID { get { return characterID; } }
    private Color transparent = new Color(0,0,0,0);
    [ContextMenu("Refresh")]
    public void Paint()
    {
        body.sprite = assets.Bodies[characterID];
        deathIcon.Clear();
    }

    public void Move(Vector3 nextPosition)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => { CameraFocus.Instance.FocusForMove(transform.position.x, transform.position.z); });
        sequence.Append(body.DOColor(transparent, ExecutionValues.Primitives.HalfAction));
        sequence.AppendCallback(() => { transform.position = nextPosition; });
        sequence.AppendCallback(() => { CameraFocus.Instance.FocusForMove(transform.position.x, transform.position.z); });
        sequence.Append(body.DOColor(Color.white, ExecutionValues.Primitives.HalfAction));
    }

    public void Initialize()
    {
        stats.Initialize(characterID.ToString());
    }

    public void PaintDeath()
    {
        deathIcon.Died();
    }
}
