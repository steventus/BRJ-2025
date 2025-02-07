using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadNote : BaseNote, IPlayerInteractable
{
    [SerializeField] private NoteType.Note noteType;

    public void OnInputDown()
    {

    }

    public void OnScratch(ScratchDirection.Direction scratchDirection)
    {
        if (isHit)
            return;

        //Debug.Log("Hitting Bad Note");

        switch (Metronome.instance.CheckIfInputIsOnBeat())
        {
            case Metronome.HitType.perfect:
                UpdateNoteHit();
                Metronome.instance.BadHit();
                break;

            case Metronome.HitType.good:
                UpdateNoteHit();
                Metronome.instance.BadHit();
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
        }
    }
    public NoteType.Note GetNoteType()
    {
        return noteType;
    }


}
