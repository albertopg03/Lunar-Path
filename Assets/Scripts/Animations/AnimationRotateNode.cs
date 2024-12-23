using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRotateNode : MonoBehaviour
{
    [SerializeField] private float durationOneLoop;

    private float rotationSpeed;
    private float rotationAmount;

    private void Start()
    {
        rotationSpeed = 360f / durationOneLoop;
    }

    private void Update()
    {
        rotationAmount = rotationSpeed * Time.deltaTime;

        transform.Rotate(0, 0, rotationAmount);
    }
}
