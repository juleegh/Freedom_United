using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMarker : MonoBehaviour
{
    [SerializeField] private SpriteRenderer icon;

    public Vector2Int GetPosition()
    {
        Vector3Int roundedPos = Vector3Int.RoundToInt(transform.position);
        return new Vector2Int(roundedPos.x, roundedPos.z);
    }

    public void Toggle(bool visible)
    {
        icon.gameObject.SetActive(visible);
    }
}
