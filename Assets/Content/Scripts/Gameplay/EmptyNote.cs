using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyNote : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] private NoteType.Note noteType;

    public void OnInputDown()
    {
    }
    public void OnScratch(ScratchDirection.Direction scratchDirection)
    {

    }

    public void OnInputUp()
    {
    }
    public void OnMiss()
    {
    }
    public NoteType.Note GetNoteType()
    {
        return noteType;
    }
}
