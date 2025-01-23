using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNote : MonoBehaviour, IPlayerInteractable, IScratchDirection
{
    public bool isRight;
    private bool isStart = true;
    private bool isHolding = false;
    private bool isHit = false;
    private HoldNote connectedEndHoldNote;

    private ScratchDirection.Direction noteDirection => isRight ? ScratchDirection.Direction.CW : ScratchDirection.Direction.ACW;


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
    }

    //Method to set "Connected Hold Note" like a linked list 
    public void ConnectEndHoldNote(HoldNote _endHoldNote)
    {
        connectedEndHoldNote = _endHoldNote;
    }

    public void OnInputDown()
    {
        if (isHit)
            return;

        //On a hold start note and you just begun holding onto it
        if (isStart && !isHolding)
        {
            isHolding = true;

            switch (Metronome.instance.CheckIfInputIsOnBeat())
            {
                case Metronome.HitType.perfect:
                    Metronome.instance.PerfectHit();
                    ReadyConnectedEndNote();
                    break;

                case Metronome.HitType.good:
                    Metronome.instance.GoodHit();
                    ReadyConnectedEndNote();
                    break;

                case Metronome.HitType.miss:
                    OnMiss();
                    break;
            }
        }

        //On a hold end note, nothing happens when you press input again
    }

    public void OnScratch(ScratchDirection.Direction scratchDirection)
    {
        if (noteDirection != scratchDirection)
        {
            OnMiss();
            return;
        }


        //On a hold start note, prematurely scratching
        if (isStart && isHolding)
        {
            Debug.Log("Miss Hold Start");
            isHolding = false;
            OnMiss();
        }

        //On a hold end note, scratching at (hopefully) right timing
        if (!isStart && isHolding)
        {
            isHit = true;
            isHolding = false;

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

    public void OnInputUp()
    {
        //On a hold start note, prematurely letting go of input
        if (isStart && isHolding)
        {
            Debug.Log("Miss Hold Start");
            isHolding = false;
            OnMiss();
        }
    }
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
        Debug.Log("Miss!");
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
}
