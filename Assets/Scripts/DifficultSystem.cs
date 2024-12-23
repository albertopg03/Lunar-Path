using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class DifficultSystem : MonoBehaviour
{
    [SerializeField] private GameLoop subjectGameLoop; 
    private int currentMinute = 0;

    [ShowInInspector]
    [DictionaryDrawerSettings(KeyLabel = "Min to spawn", ValueLabel = "Spawn Obstacles frequency")]
    private Dictionary<int, float> spawnObstaclesDifficulty = new Dictionary<int, float>
    {
        { 1, 3.0f },
        { 2, 2.5f },
        { 3, 2.2f },
        { 4, 2.0f },
        { 5, 1.8f },
        { 6, 1.5f },
        { 7, 1.0f },
        { 8, 0.8f },
        { 9, 0.6f },
        { 10, 0.5f }
    };

    [ShowInInspector]
    [DictionaryDrawerSettings(KeyLabel = "Min to spawn", ValueLabel = "Spawn PowerUps frequency")]
    private Dictionary<int, float> spawnPowerUpsDifficulty = new Dictionary<int, float>
    {
        { 1, 15.0f },
        { 2, 14.0f },
        { 3, 13.0f },
        { 4, 12.0f },
        { 5, 11.0f },
        { 6, 10.0f },
        { 7, 9.0f },
        { 8, 8.0f },
        { 9, 7.5f },
        { 10, 7.0f }
    };

    public event Action<float> OnDiffultyChangeObstacle;
    public event Action<float> OnDiffultyChangePowerUp;

    private void Init()
    {
        currentMinute = 0;
    }

    private void OnEnable()
    {
        Utils.OnMinuteChange += ChangeDifficulty;
        subjectGameLoop.OnResetGame += Init;
    }

    private void OnDisable()
    {
        Utils.OnMinuteChange -= ChangeDifficulty;
        subjectGameLoop.OnResetGame -= Init;
    }

    private void ChangeDifficulty()
    {
        currentMinute++;

        OnDiffultyChangeObstacle?.Invoke(spawnObstaclesDifficulty[currentMinute]);
        OnDiffultyChangePowerUp?.Invoke(spawnPowerUpsDifficulty[currentMinute]);
    }
}
