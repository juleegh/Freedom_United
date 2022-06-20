using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionPromptUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite up;
    [SerializeField] private Sprite down;
    [SerializeField] private Sprite left;
    [SerializeField] private Sprite right;

    public void ClearPrompt()
    {
        spriteRenderer.gameObject.SetActive(false);
    }
    
    public void SetupPrompt(Vector2Int direction)
    { 
        spriteRenderer.gameObject.SetActive(true);

        if (direction == Vector2Int.down)
            spriteRenderer.sprite = down;
        if (direction == Vector2Int.up)
            spriteRenderer.sprite = up;
        if (direction == Vector2Int.left)
            spriteRenderer.sprite = left;
        if (direction == Vector2Int.right)
            spriteRenderer.sprite = right;
    }
}
