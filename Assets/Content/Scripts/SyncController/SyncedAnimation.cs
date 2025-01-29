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
        //The error was due to conductor being disabled in the main menu, I enabled it but removed it from the turn state
        animator.Play(currentState, -1, Conductor.instance.loopPositionInBeats);
        animator.speed = 0;
    }
}
