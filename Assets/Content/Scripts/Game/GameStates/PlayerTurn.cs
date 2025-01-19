using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : BaseState
{
    [SerializeField] float stateDuration;
    [SerializeField] int successfulNotesHitCount;
    [SerializeField] int hitsRequiredToEndTurn;

    //Adding TurnTable Controls to player
    [SerializeField] TurntableManager turnTable;

    void OnEnable() {
        Events.OnSuccessfulNoteHit += CountHit;
    }
    void OnDisable() {
        Events.OnSuccessfulNoteHit -= CountHit;
    }
    public override void EnterState() {
        Debug.Log("enter " + transform.name);

        // change required number of hits based on which chart/boss is being faced against
    }
    public override void UpdateState() {
        base.UpdateState();
        
        // player input
        
        if(turnTable.Scratch()) {
            metronome.CheckIfInputIsOnBeat();
        }

        // checks player has successfully hit min number of notes
        bool hitMinimumNotes = successfulNotesHitCount >= hitsRequiredToEndTurn;
        // checks that loop has completed
        bool currentLoopIsComplete = conductor.loopPositionInAnalog <= 0.1f;

        if(hitMinimumNotes && currentLoopIsComplete) {
            isComplete = true;
        }
    }
    public override void ExitState() {
        Debug.Log("exit " + transform.name);

        successfulNotesHitCount = 0;

        conductor.completedLoops = 0;
    }

    void CountHit(int n) {
        successfulNotesHitCount++;
    }
}
