using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsFactory : Factory<PowerUp>
{
    [Header("Obstacle Types")]
    [SerializeField] private List<PowerUp> powerUpsList;

    [Header("Object Pool")]
    [SerializeField] private ObjectPoolPowerUp objectPool;

    [SerializeField] private int initialSizePool;

    private void Awake()
    {
        if (objectPool != null)
        {
            objectPool.Initialize(powerUpsList, initialSizePool);
        }
    }

    public override PowerUp Create(Vector2 position, Vector2 direction)
    {
        PowerUp powerUp = objectPool.GetRandom();
        powerUp.Init(position, direction);

        return powerUp;
    }

    public override void Return(PowerUp powerUp)
    {
        if (objectPool == null) return;

        objectPool.ReturnToPool(powerUp);
    }

    public void ResetFactory()
    {
        if (objectPool == null) return;

        foreach (var powerUp in objectPool.GetActiveObjects())
        {
            Return(powerUp);
        }
    }

}
