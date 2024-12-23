using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private PathManager pathManager;
    [SerializeField] private GameLoop subjectGameLoop;

    [Header("Settings")]
    [SerializeField] private float speed = 5f;

    private bool isReverse = false;
    public Tweener movement;

    private Vector3 targetPosition;

    public event Action<float> OnSpeedEffect;

    private void Init()
    {
        speed = 5f;

        inputHandler.ChangeDirectionEvent -= ChangeDirection;
        inputHandler.ChangeDirectionEvent += ChangeDirection;

        movement?.Kill();

        pathManager.SetNextPoint(0);
        transform.position = pathManager.GetFirstPoint();

        StartMovement();
    }

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    private void OnEnable()
    {
        movement?.Play();

        if (inputHandler != null)
        {
            subjectGameLoop.OnResetGame += Init;
            inputHandler.ChangeDirectionEvent += ChangeDirection;
        }
    }

    private void OnDisable()
    {
        movement?.Pause();
        transform.position = pathManager.GetFirstPoint();

        if (inputHandler != null)
        {
            subjectGameLoop.OnResetGame -= Init;
            inputHandler.ChangeDirectionEvent -= ChangeDirection;
        }
    }

    private void Start()
    {
        ResetPosition();
        StartMovement();
    }

    private void ChangeDirection()
    {
        isReverse = !isReverse;
        movement.Kill();

        targetPosition = pathManager.GetNextPoint(isReverse);
        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / speed;

        RotateTowards(targetPosition);
        MoveToPosition(targetPosition, duration);
    }


    private void ResetPosition()
    {
        transform.position = pathManager.GetCurrentPoint();
    }

    private void StartMovement()
    {
        targetPosition = pathManager.GetNextPoint(isReverse);
        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / speed;

        RotateTowards(targetPosition);
        MoveToPosition(targetPosition, duration);
    }

    private void MoveToPosition(Vector3 targetPosition, float duration)
    {
        movement = transform.DOMove(targetPosition, duration)
            .SetEase(Ease.Linear)
            .OnComplete(StartMovement);
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.DORotate(new Vector3(0, 0, angle), 0.25f);
    }

    private bool speedEffectActivated = false;

    public void SpeedEffect(float newSpeed, float durationEffect)
    {
        if (!speedEffectActivated)
        {
            OnSpeedEffect?.Invoke(durationEffect);
            StartCoroutine(SetSpeed(newSpeed, durationEffect));
        }
    }

    private IEnumerator SetSpeed(float newSpeed, float durationEffect)
    {
        float initSpeed = speed;
        speedEffectActivated = true;

        speed = newSpeed;

        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / speed;

        movement.ChangeValues(transform.position, targetPosition, duration);

        yield return new WaitForSeconds(durationEffect);

        speedEffectActivated = false;
        speed = initSpeed;
    }

    public void KillMovement()
    {
        movement?.Kill();
        inputHandler.ChangeDirectionEvent -= ChangeDirection;
    }
}
