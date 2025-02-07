using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : BaseState
{
    [SerializeField] float stateDuration;
    [SerializeField] int successfulNotesHitCount;
    [SerializeField] int hitsRequiredToEndTurn;

    protected override void OnEnable()
    {
        Events.OnSuccessfulNoteHit += CountHit;
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        Events.OnSuccessfulNoteHit -= CountHit;
        base.OnDisable();
    }
    public override void EnterState()
    {
        //Debug.Log("enter " + transform.name);

        //Initialise miss handling
        metronome.InitialiseMissHandling(true);

        // change required number of hits based on which chart/boss is being faced against

        HandleBossIdleDanceCue();

        //Conductor.instance.ScheduleNextPhrase(TrackFactory.instance.lengthOfChart);
    }
    public override void UpdateState()
    {
        base.UpdateState();

        #region PlayerInput


        //Clicking Disc
        if (TurntableManager.instance.OnInputDown())
        {
            //Debug.Log("Click");
            Metronome.instance.currentNote.OnInputDown();
        }

        //Scratching Disc
        ScratchDirection.Direction _scratchInput = TurntableManager.instance.ScratchInput();
        if (_scratchInput != ScratchDirection.Direction.NoScratch)
        {
            Debug.Log("Scratch direction: " + _scratchInput);
            Metronome.instance.currentNote.OnScratch(_scratchInput);
        }

        //Un-clicking Disc
        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("Unclick");
            Metronome.instance.currentNote.OnInputUp();
        }
        #endregion

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

        //Initialise miss handling
        metronome.InitialiseMissHandling(false);

        successfulNotesHitCount = 0;

        //conductor.completedLoops = 0;
    }

    void CountHit(int n)
    {
        successfulNotesHitCount++;
    }

    private void HandleBossIdleDanceCue()
    {
        bossRotationControl.currentBoss.GetComponent<BossPresenter>().CheckNoteType(NoteType.Note.empty, ScratchDirection.Direction.NoScratch);
    }
}
