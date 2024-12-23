using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private readonly Queue<Obstacle> poolQueue = new Queue<Obstacle>();
    private List<Obstacle> obstaclePrefabs;
    private int initialSize;
    private Transform parent;

    public void Initialize(List<Obstacle> obstaclePrefabs, int initialSize, Transform parent = null)
    {
        this.obstaclePrefabs = obstaclePrefabs;
        this.initialSize = initialSize;
        this.parent = !parent ? gameObject.transform : parent;

        // Primero creamos uno de cada tipo de PowerUp en la lista
        foreach (var powerUpPrefab in obstaclePrefabs)
        {
            Obstacle newObj = Instantiate(powerUpPrefab, this.parent);
            newObj.gameObject.name = "obstacle " + newObj.gameObject.GetInstanceID();
            newObj.gameObject.SetActive(false);
            poolQueue.Enqueue(newObj);
        }

        // Luego generamos objetos aleatorios hasta completar el tamaño de la pool
        int remainingSize = initialSize - obstaclePrefabs.Count;
        for (int i = 0; i < remainingSize; i++)
        {
            CreateNewObstacleInPool();
        }

        // Crear la cantidad inicial de obstáculos
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObstacleInPool();
        }
    }

    private Obstacle CreateNewObstacleInPool()
    {
        // Selecciona un prefab aleatorio y crea una instancia
        Obstacle randomPrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
        Obstacle newObj = Instantiate(randomPrefab, parent);
        newObj.gameObject.SetActive(false);
        poolQueue.Enqueue(newObj);
        return newObj;
    }

    public Obstacle Get()
    {
        var inactiveObjects = poolQueue.Where(obj => !obj.gameObject.activeInHierarchy).ToList();
        if (inactiveObjects.Count > 0)
        {
            Obstacle obj = inactiveObjects[Random.Range(0, inactiveObjects.Count)];
            obj.gameObject.SetActive(true);
            return obj;
        }

        return CreateNewObstacleInPool();
    }


    public void ReturnToPool(Obstacle obj)
    {
        obj.gameObject.SetActive(false); 
        if (!poolQueue.Contains(obj))
        {
            poolQueue.Enqueue(obj);
        }
    }


    public List<Obstacle> GetActiveObjects()
    {
        return poolQueue.Where(obj => obj.gameObject.activeInHierarchy).ToList();
    }

    public void ResetPool()
    {
        foreach (var obj in poolQueue)
        {
            obj.gameObject.SetActive(false); 
        }
    }
}
