using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDebugger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer icon;

    public void Toggle(bool visible)
    {
        icon.gameObject.SetActive(visible);
    }
}
