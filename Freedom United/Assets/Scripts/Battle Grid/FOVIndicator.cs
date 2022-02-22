using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVIndicator : MonoBehaviour
{
    [SerializeField] private GameObject fovIcon;
    [SerializeField] private GameObject hidingIcon;

    void Awake()
    {
        ToggleFOV(false);
    }

    public void ToggleFOV(bool visible)
    {
        fovIcon.SetActive(visible);
    }

    public void ToggleHiding(bool visible)
    {
        if (visible)
            ToggleFOV(false);
        hidingIcon.SetActive(visible);
    }
}
