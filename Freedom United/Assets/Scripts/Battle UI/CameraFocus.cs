using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFocus : MonoBehaviour
{
    private static CameraFocus instance;
    public static CameraFocus Instance { get { return instance; } }

    private Vector3 generalOverview;

    private void Awake()
    {
        instance = this;
        generalOverview = transform.position;
    }

    public void ClearFocus()
    {
        transform.DOMove(generalOverview, ExecutionValues.Primitives.HalfAction).SetEase(Ease.InCubic);
    }

    public void FocusForMove(float x, float y)
    {
        Vector3 newFocus = new Vector3(x, ExecutionValues.Primitives.FocusZoomDistance, y);
        transform.DOMove(newFocus, ExecutionValues.Primitives.ThirdOfAction).SetEase(Ease.InCubic);
    }

    public void FocusForAttack(List<Vector2Int> targetPositions, bool critical, bool failed)
    {
        Vector2 center = GetAveragePos(targetPositions);
        Vector3 newFocus = new Vector3(center.x, ExecutionValues.Primitives.FocusZoomDistance, center.y);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(newFocus, ExecutionValues.Primitives.SixthOfAction).SetEase(Ease.InCubic));
        if (!failed)
        {
            float shakeStrength = critical ? ExecutionValues.Primitives.CriticalAttackShake : ExecutionValues.Primitives.NormalAttackShake;
            sequence.Append(transform.DOShakePosition(ExecutionValues.Primitives.SixthOfAction, shakeStrength * Vector3.right));
        }
    }

    public void FocusForDefense(List<Vector2Int> targetPositions)
    {
        Vector2 center = GetAveragePos(targetPositions);
        Vector3 newFocus = new Vector3(center.x, ExecutionValues.Primitives.FocusZoomDistance, center.y);
        transform.DOMove(newFocus, ExecutionValues.Primitives.ThirdOfAction).SetEase(Ease.InCubic);
    }

    private Vector2 GetAveragePos(List<Vector2Int> targetPositions)
    {
        Vector2 center = Vector2.zero;

        foreach (Vector2Int pos in targetPositions)
        {
            center.x += pos.x;
            center.y += pos.y;
        }
        center.x /= targetPositions.Count;
        center.y /= targetPositions.Count;
        return center;
    }
}
