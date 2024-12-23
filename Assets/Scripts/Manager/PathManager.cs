using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private GameObject parentRoad;
    [SerializeField] private GameObject parentPoints;
    private List<GameObject> points = new List<GameObject>();
    private int nextPoint = 0;

    private void Awake()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for (int i = 0; i < parentPoints.transform.childCount; i++)
        {
            points.Add(parentPoints.transform.GetChild(i).gameObject);
        }

        CreateRoads();
    }

    public Vector3 GetNextPoint(bool isReverse)
    {
        if (isReverse)
        {
            nextPoint--;
            if (nextPoint < 0)
                nextPoint = points.Count - 1;
        }
        else
        {
            nextPoint++;
            if (nextPoint >= points.Count)
                nextPoint = 0;
        }

        return points[nextPoint].transform.position;
    }

    public Vector3 GetCurrentPoint()
    {
        return points[nextPoint].transform.position;
    }

    public Vector3 GetFirstPoint()
    {
        return points[0].transform.position;
    }

    public void SetNextPoint(int nextPoint)
    {
        this.nextPoint = nextPoint;
    }

    private void CreateRoads()
    {
        // Crear un nuevo GameObject vacío para el LineRenderer.
        GameObject lineObject = new GameObject("Road");
        lineObject.transform.SetParent(parentRoad.transform);

        // Agregar el componente LineRenderer al GameObject.
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        lineRenderer.sortingOrder = 0;

        lineRenderer.startWidth = 0.075f;
        lineRenderer.endWidth = 0.075f;

        Color roadColor = new Color(47f / 255f, 48f / 255f, 52f / 255f);

        lineRenderer.startColor = roadColor;
        lineRenderer.endColor = roadColor;

        lineRenderer.positionCount = points.Count + 1;

        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i].transform.position);
        }

        lineRenderer.SetPosition(points.Count, points[0].transform.position);
    }
}
