using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionValues : MonoBehaviour
{
    private static ExecutionValues instance;
    public static ExecutionValues Primitives {  get { return instance; } }

    [SerializeField] private float actionDuration;
    public float ActionDuration {  get { return actionDuration; } }
    [SerializeField] private float delayPercentage;
    public float DelayPercentage {  get { return delayPercentage; } }
    [SerializeField] private float regularZoomDistance;
    public float RegularZoomDistance {  get { return regularZoomDistance; } }
    [SerializeField] private float focusZoomDistance;
    public float FocusZoomDistance {  get { return focusZoomDistance; } }
    [SerializeField] private float normalAttackShake;
    public float NormalAttackShake {  get { return normalAttackShake; } }
    [SerializeField] private float criticalAttackShake;
    public float CriticalAttackShake {  get { return criticalAttackShake; } }

    private void Awake()
    {
        instance = this;
    }

    public float HalfAction { get { return actionDuration / 2 - actionDuration * delayPercentage; } }
    public float ThirdOfAction { get { return actionDuration / 3 - actionDuration * delayPercentage; } }
    public float SixthOfAction { get { return actionDuration / 6 - actionDuration * delayPercentage; } }
}
