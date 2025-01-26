using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SyncedAnimation : MonoBehaviour
{
    private Animator animator;
    private AnimatorStateInfo animatorStateInfo;
    private int currentState;
    void Start()
    {
        animator = GetComponent<Animator>();
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        currentState = animatorStateInfo.fullPathHash;
    }

    void Update()
    {
        animator.Play(currentState, -1, Conductor.instance.loopPositionInBeats);
        animator.speed = 0;
    }
}
