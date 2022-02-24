using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle
{
    private Vector2Int position;
    public Vector2Int Position { get { return position; } }

    private float hp;
    public float HP { get { return hp; } }

    public Obstacle(Vector2Int position, float hp)
    {
        this.position = position;
        this.hp = hp;
    }

    public void TakeDamage(float damageTaken)
    {
        hp -= damageTaken;
        if (hp < 0)
            hp = 0;
    }
}
