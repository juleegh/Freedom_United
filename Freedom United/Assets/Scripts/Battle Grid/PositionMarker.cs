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

    public Vector2Int Invert(int x, int y)
    {
        Vector2Int orientation = Vector2Int.left;

        Vector2Int xValue = orientation * y;
        Vector2Int yValue = new Vector2Int(orientation.y, -orientation.x) * x;

        Vector2Int temp = xValue + yValue;

        xValue = orientation * temp.y;
        yValue = new Vector2Int(orientation.y, -orientation.x) * temp.x;

        return xValue + yValue;
    }

    public void Toggle(bool visible)
    {
        icon.gameObject.SetActive(visible);
    }
}
