using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScratchDirection
{
    public enum Direction
    {
        CW,
        ACW,
        NoScratch
    }
}
public interface IPlayerInteractable
{
    //Press Input Button
    public void OnInputDown();

    public void OnScratch(ScratchDirection.Direction _direction);

    //Release Input Button
    public void OnInputUp();

    //Not pressing any button
    public void OnMiss();

    public NoteType.Note GetNoteType();

    public void UpdateNoteHit();
}
