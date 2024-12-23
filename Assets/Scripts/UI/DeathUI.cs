using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text recordText;

    [Header("References")]
    [SerializeField] private PlayerPoints points;

    private void OnEnable()
    {
        currentScoreText.text = "Your score: " + points.Points;
        recordText.text = "Your record: " + SaveSystem.GetRecord().ToString();
    }
}
