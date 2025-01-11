using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : BaseState
{
    public Transform enemy;
    [SerializeField] float stateDuration;
    public override void EnterState() {
        Debug.Log("enter " + transform.name);
    }   
    public override void UpdateState() {
        if(timeElapsed >= stateDuration) {
            isComplete = true;
        }
    }
    public override void ExitState() {
        Debug.Log("exit " + transform.name);
    }

}
