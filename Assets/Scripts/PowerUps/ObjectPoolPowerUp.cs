using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolPowerUp : MonoBehaviour
{
    private readonly Queue<PowerUp> poolQueue = new Queue<PowerUp>();
    private List<PowerUp> obstaclePrefabs;
    private int initialSize;
    private Transform parent;

    public void Initialize(List<PowerUp> obstaclePrefabs, int initialSize, Transform parent = null)
    {
        this.obstaclePrefabs = obstaclePrefabs;
        this.initialSize = initialSize;
        this.parent = !parent ? gameObject.transform : parent;

        // Primero creamos uno de cada tipo de PowerUp en la lista
        foreach (var powerUpPrefab in obstaclePrefabs)
        {
            PowerUp newObj = Instantiate(powerUpPrefab, this.parent);
            newObj.gameObject.name = "power up " + newObj.gameObject.GetInstanceID();
            newObj.gameObject.SetActive(false);
            poolQueue.Enqueue(newObj);
        }

        // Luego generamos objetos aleatorios hasta completar el tamaño de la pool
        int remainingSize = initialSize - obstaclePrefabs.Count;
        for (int i = 0; i < remainingSize; i++)
        {
            CreateNewObstacleInPool();
        }
    }

    private PowerUp CreateNewObstacleInPool()
    {
        // Selecciona un prefab aleatorio y crea una instancia
        PowerUp randomPrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
        PowerUp newObj = Instantiate(randomPrefab, parent);
        newObj.gameObject.name = "power up " + newObj.gameObject.GetInstanceID();
        newObj.gameObject.SetActive(false);
        poolQueue.Enqueue(newObj);
        return newObj;
    }

    public PowerUp GetRandom()
    {
        // Filtrar todos los objetos que no están activos
        var inactiveObjects = poolQueue.Where(obj => !obj.gameObject.activeInHierarchy).ToList();

        if (inactiveObjects.Count > 0)
        {
            // Seleccionar un objeto aleatorio de los inactivos
            var randomIndex = Random.Range(0, inactiveObjects.Count);
            var selectedObject = inactiveObjects[randomIndex];

            // Activar el objeto seleccionado y devolverlo
            selectedObject.gameObject.SetActive(true);
            return selectedObject;
        }

        // Si no hay objetos inactivos, crear uno nuevo
        return CreateNewObstacleInPool();
    }


    public void ReturnToPool(PowerUp obj)
    {
        obj.gameObject.SetActive(false);

        if (!poolQueue.Contains(obj))
        {
            poolQueue.Enqueue(obj);
        }
    }

    public List<PowerUp> GetActiveObjects()
    {
        return poolQueue.Where(obj => obj.gameObject.activeInHierarchy).ToList();
    }
}
