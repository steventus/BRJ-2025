using UnityEngine;
using System.Collections;
using System;
using UnityEditorInternal;
public class Enemy : Core
{
    public IdleState idleState;
    public AttackState attackState;
    public Attack2State attack2State;

    int attackIndex = 0;
    void Start()
    {
        SetUpStates();
        stateMachine.SetState(attackState);
    }
    void Update()
    {
        if(stateMachine.state.isComplete) {
            if(stateMachine.state == idleState) {
                stateMachine.SetState(attackState);
            }
            else {
                stateMachine.SetState(idleState);
            }
        }

       stateMachine.state.UpdateState();
    }
}