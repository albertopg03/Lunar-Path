using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPoints : MonoBehaviour
{
    [SerializeField] private int points;
    [SerializeField] private PlayerCollision subjectPlayerCollision;
    public event Action OnAddPoints;

    public UnityEvent OnAddPointsUnityEvent;

    [Header("SUBJECTS")]
    [SerializeField] private PlayerHealth subjectPlayerHealth;
    [SerializeField] private GameLoop subjectGameLoop; 

    public int Points => points;

    private void Init()
    {
        points = 0;
    }

    private void OnEnable()
    {
        subjectGameLoop.OnResetGame += Init;
    }

    private void OnDisable()
    {
        subjectGameLoop.OnResetGame -= Init;
    }

    private void Start()
    {
        subjectPlayerCollision.CollisionCheckAction += AddPoints;

        subjectPlayerHealth.OnDeath += CalculateRecord;
    }

    private void OnDestroy()
    {
        subjectPlayerCollision.CollisionCheckAction -= AddPoints;
    }

    public void AddPoints()
    {
        points++;
        OnAddPoints?.Invoke();
        OnAddPointsUnityEvent?.Invoke();
    }

    private void CalculateRecord()
    {
        int score = SaveSystem.GetRecord();

        if(points > score)
        {
            SaveSystem.SaveScore(points);
        }
    }

    private void OnApplicationQuit()
    {
        CalculateRecord();
    }
}
