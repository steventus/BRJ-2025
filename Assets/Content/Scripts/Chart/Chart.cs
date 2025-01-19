using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class Chart : MonoBehaviour
{
    public List<Note> notes = new List<Note>();
    public int chartNumOfBeats;
    private Queue<Note> arrangedNotesQueue = new Queue<Note>();

    public void SetBeats(int _numOfBeats)
    {
        chartNumOfBeats = _numOfBeats;
        GetAllChildNotes();
        ArrangeNotesForCue();
    }
    void GetAllChildNotes()
    {
        foreach (Note note in transform.GetComponentsInChildren<Note>())
        {
            notes.Add(note);
        }
    }

    void ArrangeNotesForCue()
    {
        //Based on Kato's implementation, if notes are being read from left-to-right, the arranged Notes for reading needs to be adjusted from highest note index to zero note index.
        for (int i = notes.Count - 1; i >= 0; i--)
        {
            //Get max first
            int highestNoteId = notes.Max(x => x.noteIndex);

            //Get Note with ID
            Note _selectedNote = notes.Find(x => x.noteIndex.Equals(highestNoteId));

            //Enqueue
            arrangedNotesQueue.Enqueue(_selectedNote);

            //Remove from notes List
            notes.Remove(_selectedNote);

            //Debug.Log(_selectedNote);
        }

        notes = arrangedNotesQueue.ToList();
    }

}
