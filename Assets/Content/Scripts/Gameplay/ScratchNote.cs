using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchNote : BaseNote, IPlayerInteractable, IScratchDirection
{
    public bool isRight;
    private ScratchDirection.Direction noteDirection => isRight ? ScratchDirection.Direction.CW : ScratchDirection.Direction.ACW;
    [SerializeField] private NoteType.Note noteType;

    public void OnInputDown()
    {

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

        switch (Metronome.instance.CheckIfInputIsOnBeat())
        {
            case Metronome.HitType.perfect:
                //Debug.Log("Perfect!");
                UpdateNoteHit();
                Metronome.instance.PerfectHit();
                break;

            case Metronome.HitType.good:
                //Debug.Log("Correct!");
                UpdateNoteHit();
                Metronome.instance.GoodHit();
                break;

            case Metronome.HitType.miss:
                OnMiss();
                break;
        }
    }

    public void OnInputUp()
    {

    }

    public void OnMiss()
    {
        if (!isHit)
        {
            UpdateNoteHit();
            Metronome.instance.MissHit();
            //Debug.Log("Bad!");
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

}
