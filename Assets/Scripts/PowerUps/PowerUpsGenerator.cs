using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsGenerator : MonoBehaviour
{
    public enum Direction { Up, Right, Down, Left }

    private Camera mainCamera;
    private Vector2 screenBounds;

    [Header("Factory for Power Ups")]
    [SerializeField] private float cornerMargin = 1.0f;
    [SerializeField] private float outOfBoundsOffset = 1.5f;
    [SerializeField] private float timeBetweenGenerations;
    [SerializeField] private float initTimeBetweenGenerations;
    [SerializeField] private PowerUpsFactory powerUpsFactory;

    [Header("References")]
    [SerializeField] private DifficultSystem subjectDifficultSystem;
    [SerializeField] private GameLoop subjectGameLoop;


    // [SerializeField] private float timeToDesactivateAuto; // tiempo para desactivar automáticamente por si acaso

    private void Awake()
    {
        mainCamera = Camera.main;
        screenBounds = GetScreenBounds();
    }

    private void OnEnable()
    {
        subjectGameLoop.OnResetGame += Init;
        subjectDifficultSystem.OnDiffultyChangePowerUp += ChangeSpeedGeneration;

        Init();
    }

    private void OnDisable()
    {
        subjectGameLoop.OnResetGame -= Init;
        subjectDifficultSystem.OnDiffultyChangePowerUp -= ChangeSpeedGeneration;
    }

    private void Init()
    {
        StopAllCoroutines(); 
        timeBetweenGenerations = initTimeBetweenGenerations;

        if (powerUpsFactory != null)
        {
            powerUpsFactory.ResetFactory();
        }

        StartCoroutine(GenerateObstaclesCoroutine()); 
    }


    private void Start()
    {
        initTimeBetweenGenerations = timeBetweenGenerations;

        StartCoroutine(GenerateObstaclesCoroutine());
    }

    private Vector2 GetScreenBounds()
    {
        return new Vector2(
            (mainCamera.orthographicSize * mainCamera.aspect) - mainCamera.orthographicSize * mainCamera.aspect / 5,
            mainCamera.orthographicSize - (mainCamera.orthographicSize / 5)
        );
    }

    private IEnumerator GenerateObstaclesCoroutine()
    {
        while (true)
        {
            GenerateObstacle();
            yield return new WaitForSeconds(timeBetweenGenerations);
        }
    }

    private void GenerateObstacle()
    {
        if (powerUpsFactory == null) return;

        PositionDirection posDir = GetRandomPositionDirection();
        PowerUp powerUp = powerUpsFactory.Create(posDir.position, posDir.direction);

        //StartCoroutine(DeactivateAfterTime(powerUp, timeToDesactivateAuto));
    }

    private IEnumerator DeactivateAfterTime(PowerUp powerUp, float time)
    {
        yield return new WaitForSeconds(time);

        // Desactiva el obstáculo y lo devuelve al pool
        powerUpsFactory.Return(powerUp);
    }

    private PositionDirection GetRandomPositionDirection()
    {
        Direction randomDirection = GetRandomDirection();
        float x = 0, y = 0;
        Vector2 direction = Vector2.zero;

        switch (randomDirection)
        {
            case Direction.Up:
                x = Random.Range(-screenBounds.x + cornerMargin, screenBounds.x - cornerMargin);
                y = screenBounds.y + outOfBoundsOffset; 
                direction = Vector2.down;
                break;
            case Direction.Right:
                x = screenBounds.x + outOfBoundsOffset; 
                y = Random.Range(-screenBounds.y + cornerMargin, screenBounds.y - cornerMargin);
                direction = Vector2.left;
                break;
            case Direction.Down:
                x = Random.Range(-screenBounds.x + cornerMargin, screenBounds.x - cornerMargin);
                y = -screenBounds.y - outOfBoundsOffset; 
                direction = Vector2.up;
                break;
            case Direction.Left:
                x = -screenBounds.x - outOfBoundsOffset; 
                y = Random.Range(-screenBounds.y + cornerMargin, screenBounds.y - cornerMargin);
                direction = Vector2.right;
                break;
        }

        return new PositionDirection(new Vector2(x, y), direction);
    }


    public Direction GetRandomDirection()
    {
        Direction[] directions = (Direction[])System.Enum.GetValues(typeof(Direction));
        return directions[Random.Range(0, directions.Length)];
    }

    private struct PositionDirection
    {
        public Vector2 position;
        public Vector2 direction;

        public PositionDirection(Vector2 pos, Vector2 dir)
        {
            position = pos;
            direction = dir;
        }
    }

    private void ChangeSpeedGeneration(float speed)
    {
        timeBetweenGenerations = speed;
    }
}
