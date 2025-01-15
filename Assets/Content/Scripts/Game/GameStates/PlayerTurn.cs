using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : BaseState
{
    [SerializeField] float stateDuration;
    public override void EnterState() {
        Debug.Log("enter " + transform.name);
    }
    public override void UpdateState() {
        base.UpdateState();

        if(timeElapsed >= stateDuration) {
            isComplete = true;
        }
    }
    public override void ExitState() {
        Debug.Log("exit " + transform.name);
    }
}
