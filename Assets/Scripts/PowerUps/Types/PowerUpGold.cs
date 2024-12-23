using UnityEngine;

public class PowerUpGold : PowerUp
{
    private PlayerMovement move;
    private PlayerHealth health;

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
        health = target.GetComponent<PlayerHealth>();

        move.SpeedEffect(speedEffect, durationEffect);
        health.InmunityEffect(durationEffect);
    }
}
