using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : BaseState
{
    [SerializeField] float stateDuration;

    [Header("Game Components")]
    public MusicManager musicManager;
    
    public override void EnterState() {
        Debug.Log("enter " + transform.name);

        musicManager.StartFade();
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
