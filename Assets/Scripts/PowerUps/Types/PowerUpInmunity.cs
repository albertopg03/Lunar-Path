using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInmunity : PowerUp
{
    private PlayerHealth health;

    [Header("POWER UP EFFECTS")]
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
        health = target.GetComponent<PlayerHealth>();

        health.InmunityEffect(durationEffect);
    }
}
