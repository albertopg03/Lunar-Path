using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsGenerator : MonoBehaviour
{

    private Camera mainCamera;

    [SerializeField] private float offset;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        GenerateBounds("BOUND UP", new Vector2(0, GetScreenBounds().y + offset), new Vector2(GetScreenBounds().x * 2, 1) * 2);
        GenerateBounds("BOUND RIGT", new Vector2(GetScreenBounds().x + offset, 0), new Vector2(1, GetScreenBounds().y * 2) * 2);
        GenerateBounds("BOUND DOWN", new Vector2(0, -GetScreenBounds().y - offset), new Vector2(GetScreenBounds().x * 2, 1) * 2);
        GenerateBounds("BOUND LEFT", new Vector2(-GetScreenBounds().x - offset,0), new Vector2(1, GetScreenBounds().y * 2) * 2);
    }

    private Vector2 GetScreenBounds()
    {
        return new Vector2(
            mainCamera.orthographicSize * mainCamera.aspect,
            mainCamera.orthographicSize
        );
    }

    private void GenerateBounds(string name, Vector2 position, Vector2 size)
    {
        GameObject boundObject = new GameObject(name);

        BoxCollider2D colliderBound = boundObject.AddComponent<BoxCollider2D>();
        colliderBound.isTrigger = true;
        colliderBound.size = size;

        Bound bound = boundObject.AddComponent<Bound>();
        bound.transform.position = position;

        boundObject.transform.SetParent(transform);
    }
}
