using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationExecute : MonoBehaviour
{
    [SerializeField] private string triggerName;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Execute()
    {
        anim.SetTrigger(triggerName);
    }
}
