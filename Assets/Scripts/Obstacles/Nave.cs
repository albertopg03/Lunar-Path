using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nave : Obstacle, IRandomScalable, IOrientable
{
    public override void InitializeObstacle(Vector2 position, Vector2 direction)
    {
        transform.position = position;
        Move(direction);
        RandomScale(0.75f, 1.75f);
        LookAt(direction);
    }

    public void LookAt(Vector2 moveDirection)
    {
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public void RandomScale(float min, float max)
    {
        float scale = Random.Range(min, max);
        transform.localScale = new Vector2(scale, scale);
    }
}
