using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IHEalthProvider
{

    [SerializeField] private int initLives = 3;
    [SerializeField] private int currentLives;

    private bool isInmune;

    public int InitiLives => initLives;
    public int CurrentLives => initLives;

    [Header("References")]
    [SerializeField] private PlayerCollision subjectPlayerCollision; 
    [SerializeField] private GameLoop subjectGameLoop;

    public event Action<int> OnLivesChanged;
    public event Action<float> OnGodMode;
    public event Action OnDamage;

    public event Action OnDeath;
    public UnityEvent OnDeathUnityEvent;

    private void OnEnable()
    {
        subjectGameLoop.OnResetGame += Init;
    }

    private void OnDisable()
    {
        subjectGameLoop.OnResetGame -= Init;
    }

    private void Init()
    {
        currentLives = initLives;
        isInmune = false;
    }

    private void Awake()
    {
        subjectPlayerCollision.CollisionAction += TakeLife;
        currentLives = initLives;
    }

    private void OnDestroy()
    {
        subjectPlayerCollision.CollisionAction -= TakeLife;
    }

    private void TakeLife()
    {
        if (!isInmune)
        {
            OnDamage?.Invoke();

            currentLives -= 1;
            OnLivesChanged?.Invoke(currentLives);

            if (currentLives <= 0)
                Death();
        }
    }

    public void InmunityEffect(float duration)
    {
        OnGodMode?.Invoke(duration);
        StartCoroutine(SetInmunity(duration));
    }

    private IEnumerator SetInmunity(float duration)
    {
        isInmune = true;

        yield return new WaitForSeconds(duration);

        isInmune = false;
    }

    private void Death()
    {
        OnDeath?.Invoke();
        OnDeathUnityEvent?.Invoke();
    }
}
