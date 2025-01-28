using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

//TODO: TO BE INTEGRATED WITH KATO'S NOTE TYPES
public class NoteType
{
    public enum Note
    {
        scratch,
        holdStart,
        holdEnd,
        bad,
        change,
        empty
    }
}
public class BossPresenter : MonoBehaviour
{
    public Sprite scratchNoteSprite;
    public Sprite holdStartNoteSprite;
    public Sprite holdEndNoteSprite;
    public Sprite badNoteSprite;
    public Sprite changeNoteSprite;
    public Sprite emptyNoteSprite;
    public Sprite UIPortraitSprite;

    public enum DancePose
    {
        scratchNote,
        holdStartNote,
        holdEndNote,
        badNote,
        changeNote,
        emptyNote
    }
    Dictionary<NoteType.Note, DancePose> danceDictionary = new Dictionary<NoteType.Note, DancePose>(){
        {NoteType.Note.scratch, DancePose.scratchNote},
        {NoteType.Note.holdStart, DancePose.holdStartNote},
        {NoteType.Note.holdEnd, DancePose.holdEndNote},
        {NoteType.Note.bad, DancePose.badNote},
        {NoteType.Note.change, DancePose.changeNote},
        {NoteType.Note.empty, DancePose.emptyNote}
    };

    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {

    }

    //When chart is created with notes being filled in, run this method for each note filling in.
    public void CheckNoteType(NoteType.Note noteType, ScratchDirection.Direction _direction)
    {
        //Todo: Need to be connected with Kato's chart loader - so the dance will cue appropriately
        DancePose _chosenDance;
        if (danceDictionary.TryGetValue(noteType, out _chosenDance))
        {
            PerformDance(_chosenDance, _direction);
        }

        else Debug.Log("Error no appropriate dance pose found!");
    }

    public void PerformDance(DancePose dancePose, ScratchDirection.Direction _direction)
    {
        spriteRenderer.flipX = _direction == ScratchDirection.Direction.CW ? true : false;;

        //Choose corresponding sprite based on dancePose received
        switch (dancePose)
        {
            case DancePose.scratchNote:
                spriteRenderer.sprite = scratchNoteSprite;
                break;

            case DancePose.holdStartNote:
                spriteRenderer.sprite = holdStartNoteSprite;
                break;

            case DancePose.holdEndNote:
                spriteRenderer.sprite = holdEndNoteSprite;
                break;

            case DancePose.badNote:
                spriteRenderer.sprite = badNoteSprite;
                break;

            case DancePose.changeNote:
                spriteRenderer.sprite = changeNoteSprite;
                break;

            case DancePose.emptyNote:
                spriteRenderer.sprite = emptyNoteSprite;
                break;
        }
    }
}
