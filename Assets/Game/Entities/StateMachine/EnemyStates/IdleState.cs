using UnityEngine;

public class IdleState : BaseState 
{
    [SerializeField] float idleStateLength;
    public override void EnterState() {
        Debug.Log("Idle");
    }
    public override void UpdateState() {
        if(timeElapsed >= idleStateLength) {
            isComplete = true;
        }
    }
    public override void ExitState() {
    }
}