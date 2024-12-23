using System.Collections;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{
    public enum Direction { Up, Right, Down, Left }

    private Camera mainCamera;
    private Vector2 screenBounds;

    [Header("Factory for Obstacles")]
    [SerializeField] private float cornerMargin = 1.0f;
    [SerializeField] private float outOfBoundsOffset = 1.0f; 
    [SerializeField] private float timeBetweenGenerations;
    [SerializeField] private float initTimeBetweenGenerations;
    [SerializeField] private ObstacleFactory obstacleFactory;

    [SerializeField] private float timeToDesactivateAuto;

    [Header("References")]
    [SerializeField] private DifficultSystem subjectDifficultSystem;
    [SerializeField] private GameLoop subjectGameLoop;

    private void Awake()
    {
        mainCamera = Camera.main;
        screenBounds = GetScreenBounds();
    }

    private void OnEnable()
    {
        subjectGameLoop.OnResetGame += Init;
        subjectDifficultSystem.OnDiffultyChangeObstacle += ChangeSpeedGeneration;

        Init();
    }

    private void OnDisable()
    {
        subjectGameLoop.OnResetGame -= Init;
        subjectDifficultSystem.OnDiffultyChangeObstacle -= ChangeSpeedGeneration;
    }

    private void Init()
    {
        StopAllCoroutines(); 
        timeBetweenGenerations = initTimeBetweenGenerations;

        if (obstacleFactory != null)
        {
            obstacleFactory.ResetFactory(); 
        }

        StartCoroutine(GenerateObstaclesCoroutine()); 
    }


    private void Start()
    {
        StartCoroutine(GenerateObstaclesCoroutine());
    }

    private Vector2 GetScreenBounds()
    {
        /* cálculos exáctos para la obtención de la medida de la pantalla
        return new Vector2(
            mainCamera.orthographicSize * mainCamera.aspect,
            mainCamera.orthographicSize
        );
        */
        // cálculo para acotar un poco los bordes posibles de generación de los objetos
        return new Vector2(
            (mainCamera.orthographicSize * mainCamera.aspect) - mainCamera.orthographicSize * mainCamera.aspect / 9,
            mainCamera.orthographicSize - (mainCamera.orthographicSize / 9)
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
        if (obstacleFactory == null) return;

        PositionDirection posDir = GetRandomPositionDirection();
        Obstacle obstacle = obstacleFactory.Create(posDir.position, posDir.direction);

        StartCoroutine(DeactivateAfterTime(obstacle, timeToDesactivateAuto));
    }

    private IEnumerator DeactivateAfterTime(Obstacle obstacle, float time)
    {
        yield return new WaitForSeconds(time);

        // Desactiva el obstáculo y lo devuelve al pool
        obstacleFactory.Return(obstacle);
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
