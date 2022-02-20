using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVIndicator : MonoBehaviour
{
    [SerializeField] private GameObject fovIcon;

    void Awake()
    {
        ToggleFOV(false);
    }

    public void ToggleFOV(bool visible)
    {
        fovIcon.SetActive(visible);
    }
}
