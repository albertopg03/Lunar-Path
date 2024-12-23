using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFactory : Factory<Obstacle>
{
    [Header("Obstacle Types")]
    [SerializeField] private List<Obstacle> obstacles;

    [Header("Object Pool")]
    [SerializeField] private ObjectPool objectPool;

    [SerializeField] private int initialSizePool;

    private void Awake()
    {
        if (objectPool != null)
        {
            objectPool.Initialize(obstacles, initialSizePool);
        }
    }

    public override Obstacle Create(Vector2 position, Vector2 direction)
    {
        Obstacle obstacle = objectPool.Get();
        obstacle.InitializeObstacle(position, direction);

        return obstacle;
    }

    public override void Return(Obstacle obstacle)
    {
        if (objectPool != null) return;

        objectPool.ReturnToPool(obstacle);
    }

    public void ResetFactory()
    {
        if (objectPool == null) return;

        objectPool.ResetPool(); 
    }
}
