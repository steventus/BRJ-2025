using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : BaseState
{
    [SerializeField] float stateDuration;
    [SerializeField] int successfulNotesHitCount;
    [SerializeField] int hitsRequiredToEndTurn;

    void OnEnable()
    {
        Events.OnSuccessfulNoteHit += CountHit;
    }
    void OnDisable()
    {
        Events.OnSuccessfulNoteHit -= CountHit;
    }
    public override void EnterState()
    {
        Debug.Log("enter " + transform.name);

        //Initialise miss handling
        metronome.InitialiseMissHandling(true);

        // change required number of hits based on which chart/boss is being faced against
    }
    public override void UpdateState()
    {
        base.UpdateState();

        // player input

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //metronome.CheckIfInputIsOnBeat();
            Metronome.instance.currentNote.OnInputDown();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Metronome.instance.currentNote.OnInputUp();
        }

        // checks player has successfully hit min number of notes
        bool hitMinimumNotes = successfulNotesHitCount >= hitsRequiredToEndTurn;
        // checks that loop has completed
        bool currentLoopIsComplete = conductor.loopPositionInAnalog <= 0.1f;

        if (hitMinimumNotes && currentLoopIsComplete)
        {
            isComplete = true;
        }
    }
    public override void ExitState()
    {
        Debug.Log("exit " + transform.name);

        //Initialise miss handling
        metronome.InitialiseMissHandling(false);

        successfulNotesHitCount = 0;

        conductor.completedLoops = 0;
    }

    void CountHit(int n)
    {
        successfulNotesHitCount++;
    }
}
