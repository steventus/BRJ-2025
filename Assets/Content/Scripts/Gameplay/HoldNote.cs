using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNote : MonoBehaviour, IPlayerInteractable, IScratchDirection
{
    public bool isCW;
    private bool isStart = true;
    public bool IsStart => isStart;
    private bool isHolding = false;
    private bool isHit = false;
    private HoldNote connectedEndHoldNote;
    public HoldNote ConnectedEndHoldNote => connectedEndHoldNote;

    private ScratchDirection.Direction noteDirection => isCW ? ScratchDirection.Direction.CW : ScratchDirection.Direction.ACW;
    [SerializeField] private NoteType.Note noteType;

    // ## JOHNNY ## Refactoring to be scratch on first note, hold mouse button down until end note, release at correct timing for perfect
    // ====================================================================================

    //adding reference to start hold note for end note to check if prev note was held
    private HoldNote connectedStartHoldNote;

    // ====================================================================================

    //Called and set from TrackFactory
    public void SetStartHold()
    {
        //If it is already an start note, skip everything
        if (isStart)
        {
            Debug.Log("Error, current hold note is already a start note");
            return;
        }

        isStart = true;
        noteType = NoteType.Note.holdStart;
    }
    //Called and set from TrackFactory
    public void SetEndHold()
    {
        //If it is already an end note, skip everything
        if (!isStart)
        {
            Debug.Log("Error, current hold note is already an end note");
            return;
        }

        isStart = false;
        noteType = NoteType.Note.holdEnd;
    }

    //Method to set "Connected Hold Note" like a linked list 
    public void ConnectEndHoldNote(HoldNote _endHoldNote)
    {
        connectedEndHoldNote = _endHoldNote;

        // ## JOHNNY ## Refactoring to be scratch on first note, hold mouse button down until end note, release at correct timing for perfect

        _endHoldNote.connectedStartHoldNote = this;
        Debug.Log(_endHoldNote.connectedStartHoldNote);

        // ## JOHNNY ## Refactoring to be scratch on first note, hold mouse button down until end note, release at correct timing for perfect
    }

    public void OnInputDown()
    {
        if (isHit)
            return;

        // ## JOHNNY ## Refactoring to be scratch on first note, hold mouse button down until end note, release at correct timing for perfect
        // ====================================================================================

        //On a hold start note and you just begun holding onto it
        if (isStart && !isHolding)
        {
            isHolding = true;

            //     switch (Metronome.instance.CheckIfInputIsOnBeat())
            //     {
            //         case Metronome.HitType.perfect:
            //             Metronome.instance.PerfectHit();
            //             ReadyConnectedEndNote();
            //             break;

            //         case Metronome.HitType.good:
            //             Metronome.instance.GoodHit();
            //             ReadyConnectedEndNote();
            //             break;

            //         case Metronome.HitType.miss:
            //             OnMiss();
            //             break;
            //     }
        }

        //On a hold end note, nothing happens when you press input again

        // ## JOHNNY ## Refactoring to be scratch on first note, hold mouse button down until end note, release at correct timing for perfect
        // ====================================================================================
    }

    public void OnScratch(ScratchDirection.Direction scratchDirection)
    {
        if (isHit)
            return;

        if (noteDirection != scratchDirection)
        {
            OnMiss();
            return;
        }


        // ## JOHNNY ## Refactoring to be scratch on first note, hold mouse button down until end note, release at correct timing for perfect
        // ====================================================================================

        //check if scratch correctly timed on start note
        if (isStart)
        {
            isHit = true;
            Debug.Log("hold note scratch");
            switch (Metronome.instance.CheckIfInputIsOnBeat())
            {
                case Metronome.HitType.perfect:
                    Metronome.instance.PerfectHit();
                    break;

                case Metronome.HitType.good:
                    Metronome.instance.GoodHit();
                    break;

                case Metronome.HitType.miss:
                    OnMiss();
                    break;
            }
        }
        // ====================================================================================

    }

    public void OnInputUp()
    {
        if (isHit)
            return;

        isHit = true;

        //On a hold start note, prematurely letting go of input
        if (noteType == NoteType.Note.holdStart)
        {
            Debug.Log("Miss Hold Start");
            isHolding = false;
            OnMiss();
        }
        // ## JOHNNY ## Refactoring to be scratch on first note, hold mouse button down until end note, release at correct timing for perfect
        // ====================================================================================
        else if (noteType == NoteType.Note.holdEnd && connectedStartHoldNote.GetIsHolding())
        {
            Debug.Log("hold note release");
            switch (Metronome.instance.CheckIfInputIsOnBeat())
            {
                case Metronome.HitType.perfect:
                    Metronome.instance.PerfectHit();
                    break;

                case Metronome.HitType.good:
                    Metronome.instance.GoodHit();
                    break;

                case Metronome.HitType.miss:
                    OnMiss();
                    break;
            }
        }

    }

    // ## JOHNNY ## Refactoring to be scratch on first note, hold mouse button down until end note, release at correct timing for perfect
    // ====================================================================================

    public void OnMiss()
    {
        //Natural guard
        if (isHit)
            return;

        //Guard if holding while its a hold start note
        if (isStart && isHolding)
            return;

        //Disable hold note
        isHit = true;

        //Disable connected end hold note at the same time
        if (isStart && connectedEndHoldNote != null)
            connectedEndHoldNote.isHit = true;

        Metronome.instance.MissHit();
        //Debug.Log("Miss!");
    }

    private void ReadyConnectedEndNote()
    {
        if (isStart && connectedEndHoldNote != null)
        {
            connectedEndHoldNote.isHolding = true;
        }
    }

    public ScratchDirection.Direction GetScratchDirection()
    {
        return noteDirection;
    }

    public NoteType.Note GetNoteType()
    {
        return noteType;
    }

    public bool GetIsHolding()
    {
        return isHolding;
    }
}
