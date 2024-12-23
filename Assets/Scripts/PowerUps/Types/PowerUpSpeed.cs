using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : PowerUp
{
    private PlayerMovement move;

    [Header("POWER UP EFFECTS")]
    [SerializeField] private float speedEffect;
    [SerializeField] private float durationEffect;

    public override void Init(Vector2 position, Vector2 direction)
    {
        transform.position = position;
        Move(direction);
    }

    public override void MakeEffect(GameObject target)
    {
        Effect(target);
    }

    private void Effect(GameObject target)
    {
        move = target.GetComponent<PlayerMovement>();

        move.SpeedEffect(speedEffect, durationEffect);
    }
}
