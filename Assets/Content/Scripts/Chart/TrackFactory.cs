using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TrackFactory : MonoBehaviour
{
    public static TrackFactory instance = null;
    [Header("Notes Prefabs")]
    public GameObject changeNotePrefab;
    public GameObject scratchNotePrefab;
    public GameObject holdNotePrefab;
    public GameObject badNotePrefab;
    public GameObject emptyNotePrefab;

    [Header("Chart, Track and Gameplay")]
    public RectTransform track;
    public GameObject currentChartPrefab;
    private Chart currentChart => currentChartPrefab.GetComponent<Chart>();
    public List<GameObject> notes;

    // [[ JOHNNY - adding charts to spawn ]]
    // ================================================================ //

    public Chart chartToSpawn;

    // ================================================================ //

    [Header("Dynamic Track Generation Settings")]
    //Set Track Length
    public float MinLength; //Base length at minimum notes
    public float LengthPerBeat; //Additional length (gradient) per additional notes
    public float trackHeight;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            CreateTrack();
    }

    public void CreateTrack()
    {
        ClearTrack();

        //Receive data from ChartMaker and instantiate new notes under track
        // [[ JOHNNY - adding charts to spawn ]]
        // ================================================================ //
        for (int i = 0; i <= chartToSpawn.notes.Count - 1; i++)
        {
            Note _currentNote = currentChart.notes[i];
            GameObject _instantiatedNote = null;
            switch (_currentNote.noteType)
            {
                case NoteType.Note.scratch:
                    _instantiatedNote = Instantiate(scratchNotePrefab, track);

                    break;
                case NoteType.Note.holdStart:
                    //Instantiate both start and end hold notes
                    _instantiatedNote = Instantiate(holdNotePrefab, track);
                    _instantiatedNote.GetComponent<HoldNote>().SetStartHold();


                    break;
                case NoteType.Note.holdEnd:
                    _instantiatedNote = Instantiate(holdNotePrefab, track);
                    _instantiatedNote.GetComponent<HoldNote>().SetEndHold();
                    break;
                case NoteType.Note.change:
                    _instantiatedNote = Instantiate(changeNotePrefab, track);

                    break;
                case NoteType.Note.bad:
                    _instantiatedNote = Instantiate(badNotePrefab, track);

                    break;
                default:
                    _instantiatedNote = Instantiate(emptyNotePrefab, track);
                    break;
            }

            notes.Add(_instantiatedNote);
        }

        //ApplyHoldNotes();

        AdjustTrackLength();
    }

    //public void ApplyHoldNotes()
    //{
    //    for (int i = 0; i <= notes.Count; i++)
    //    {
    //        Note _currentNote = notes[i].GetComponent<Note>();

    //        //Check if current selected note is connected. 
    //        if (_currentNote.isConnected)
    //        {
    //            //If it is, additionally check if its already assigned.
    //            if (_currentNote.noteType != NoteType.Note.holdStart || _currentNote.noteType != NoteType.Note.holdEnd)
    //                continue;

    //            Note _connectedNote = notes[_currentNote.connectedNoteIndex].GetComponent<Note>();

    //            _currentNote.SetNoteType(NoteType.Note.holdStart);
    //            _connectedNote.SetNoteType(NoteType.Note.holdEnd);
    //        }
    //    }
    //}

    void AdjustTrackLength()
    {
        //Adjust distance
        GetComponent<RectTransform>().sizeDelta = new Vector2(MinLength + notes.Count * LengthPerBeat, trackHeight);
    }

    void ClearTrack()
    {
        for (int i = notes.Count - 1; i >= 0; i--)
        {
            Destroy(notes[i].gameObject);
        }

        notes.Clear();
    }

    void OnValidate()
    {
        AdjustTrackLength();
    }
}
