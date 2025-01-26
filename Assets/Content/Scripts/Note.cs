using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public bool isPicked;
    public bool isStart, isEnd;

    public bool isInTrack;
    public bool isSelected;
    public bool isConnected;

    public bool isOccupied = false;
    public int noteIndex;
    public int connectedNoteIndex;

    [Space]
    [Header("Cue System Variables")]
    //Data for what type of note this is
    public NoteType.Note noteType;

    //Data to be fed into BossPresenter.cs
    public bool ifRight;

    public void SetNoteType(NoteType.Note _noteType){
        noteType = _noteType;
    }
}
