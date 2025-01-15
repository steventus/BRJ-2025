using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : BaseState
{
    [SerializeField] float stateDuration;
    [SerializeField] int successfulNotesHitCount;
    void OnEnable() {
        metronome.OnSuccessfulNoteHit += AddToHitCounter;
    } 
    void OnDisable() {
        metronome.OnSuccessfulNoteHit -= AddToHitCounter;
    }
    public override void EnterState() {
        metronome.isPlayerTurn = true;
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

        successfulNotesHitCount = 0;
        metronome.isPlayerTurn = false;
    }

    void AddToHitCounter() {
        successfulNotesHitCount++;
    }
}
