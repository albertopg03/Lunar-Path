using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Obstacle, IRandomScalable
{
    public override void InitializeObstacle(Vector2 position, Vector2 direction)
    {
        transform.position = position;
        Move(direction);
        RandomScale(0.8f, 1.5f);
    }

    public void RandomScale(float min, float max)
    {
        float scale = Random.Range(min, max);
        transform.localScale = new Vector2(scale, scale);
    }
}

