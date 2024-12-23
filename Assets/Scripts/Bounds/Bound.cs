using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    public event Action<GameObject> CollisionBoundAction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            if (collision.TryGetComponent(out Obstacle obstacle))
            {
                CollisionBoundAction?.Invoke(obstacle.gameObject);
            }
        }

        if (collision.CompareTag("PowerUp"))
        {
            if (collision.TryGetComponent(out PowerUp powerUp))
            {
                CollisionBoundAction?.Invoke(powerUp.gameObject);
            }
        }
    }
}
