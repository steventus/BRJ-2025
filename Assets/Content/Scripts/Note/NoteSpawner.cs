using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

public class NoteSpawner : MonoBehaviour
{
    public GameObject scratchL, scratchR, bossChange, avoid, emptyNote;
    private Track[] tracks;
    public LayerMask whatIsNote;

    public TMP_InputField inputField;

    public int[] notesPerBeat;
    public int[] trackNum;
    public int[] noteTypes;

    public float noteOffset;

    List<GameObject> notePositions = new List<GameObject>();
    List<GameObject> activeNotes = new List<GameObject>();

    private RaycastHit2D hit;
    private Vector3 mousePosRaw;
    private Vector2 mousePos;

    GameObject tmpNote = null;

    GameObject wholeChart;

    private int uniChartID = 0;

    bool isPlaying;

    int notesSelected = 0;

    private bool hasPickedUpNote = false;
    private void Start()
    {

        wholeChart = new GameObject();
        tracks = GameObject.FindObjectsOfType<Track>();
        wholeChart.transform.position = tracks[0].endPos;

        wholeChart.AddComponent<Chart>();
    }

    private void Update()
    {
        mousePosRaw = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePosRaw.x, mousePosRaw.y, 0f);

        hit = Physics2D.Raycast(mousePosRaw, Vector3.zero, Mathf.Infinity, whatIsNote);

        if (Input.GetMouseButton(0) && Physics2D.Raycast(mousePosRaw, Vector3.zero, Mathf.Infinity, whatIsNote))
        {

            if (!hit.transform.gameObject.GetComponent<Note>().isInTrack && !hit.transform.gameObject.GetComponent<Note>().isPicked)
                CreateNote();
        }
        if (hasPickedUpNote)
        {
            DragNote();
        }

        if (Input.GetMouseButtonUp(0) && Physics2D.Raycast(mousePosRaw, Vector3.zero, Mathf.Infinity, whatIsNote) && hasPickedUpNote)
        {
            hasPickedUpNote = false;
            CheckClosestEmptyNote(tmpNote);
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveAllNotes(1);
        }
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveAllNotes(-1);
        }

        if (isPlaying)
        {
            Play();
        }

        if (Input.GetMouseButtonDown(0) && Physics2D.Raycast(mousePosRaw, Vector3.zero, Mathf.Infinity, whatIsNote) && hit.transform.gameObject.GetComponent<Note>().isInTrack && !hit.transform.gameObject.GetComponent<Note>().isSelected && !hit.transform.gameObject.GetComponent<Note>().isConnected)
        {
            hit.transform.gameObject.GetComponent<Note>().isSelected = true;
            notesSelected++;
        }

        if (notesSelected == 2)
        {
            notesSelected = 0;
            MakeHoldNotes();
        }

        CheckIfEmptyNoteIsOccupied();
    }

    //used when building the chart
    void MoveAllNotes(int dir)
    {
        wholeChart.transform.position += new Vector3(noteOffset * dir, 0, 0);
    }

    //gotta work on this one(fking notes don't follow me if i move too fast lol)
    void DragNote()
    {
        if (hasPickedUpNote)
        {
            tmpNote.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
        }
    }

    void CreateNote()
    {
        if (!hasPickedUpNote)
        {
            tmpNote = Instantiate(hit.transform.gameObject, mousePos, Quaternion.identity);
            tmpNote.transform.GetComponent<Note>().isPicked = true;
            hasPickedUpNote = true;
            activeNotes.Add(tmpNote);
        }
        else return;
    }

    //checks the closest track to the picked note
    void CheckClosestEmptyNote(GameObject note)
    {
        float closestDistance = Mathf.Infinity;
        int closestEmptyNoteID = -1;

        for (int i = 0; i < notePositions.Count; i++)
        {
            if (Vector3.Distance(notePositions[i].transform.position, note.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(notePositions[i].transform.position, note.transform.position);
                closestEmptyNoteID = i;
            }
        }
        if (!notePositions[closestEmptyNoteID].GetComponent<Note>().isOccupied)
        {
            note.transform.position = notePositions[closestEmptyNoteID].transform.position;
            note.transform.parent = wholeChart.transform;
            note.GetComponent<Note>().isPicked = false;
            note.GetComponent<Note>().isInTrack = true;
            note.GetComponent<Note>().noteIndex = notePositions[closestEmptyNoteID].GetComponent<Note>().noteIndex;
            notePositions[closestEmptyNoteID].GetComponent<Note>().isOccupied = true;
        }
        else
        {
            Destroy(note);
            Debug.Log("Empty Note Already Occupied");
        }
    }

    void CheckIfEmptyNoteIsOccupied()
    {
        RaycastHit2D hit;


        for (int i = 0; i < tracks.Length; i++)
        {
            hit = Physics2D.Raycast(tracks[i].endPos, Vector3.zero, Mathf.Infinity, whatIsNote);

            //Debug.Log(tracks[i].isOccupied);

            if (hit && !hit.transform.GetComponent<Note>().isPicked)
                tracks[i].isOccupied = true;
            else if (hit && hit.transform.GetComponent<Note>().isPicked)
                tracks[i].isOccupied = false;
            else if (!hit)
                tracks[i].isOccupied = false;
        }
    }

    void MakeHoldNotes()
    {
        Note[] notes = new Note[2];

        LineRenderer lr;

        for (int i = 0; i < notes.Length; i++)
        {
            for (int j = 0; j < activeNotes.Count; j++)
            {
                if (activeNotes[j].GetComponent<Note>().isSelected)
                {
                    notes[i] = activeNotes[j].GetComponent<Note>();
                    activeNotes[j].GetComponent<Note>().isSelected = false;
                    break;
                }
            }
        }

        notes[0].connectedNoteIndex = notes[1].noteIndex;
        notes[1].connectedNoteIndex = notes[0].noteIndex;
        notes[0].isConnected = true;
        notes[1].isConnected = true;

        if (notes[0].noteIndex < notes[1].noteIndex)
        {
            notes[0].isEnd = true;
            notes[1].isStart = true;
        }
        else
        {
            notes[0].isStart = true;
            notes[1].isEnd = true;
        }

        lr = notes[0].gameObject.AddComponent<LineRenderer>();
        lr.SetPosition(0, notes[0].transform.position);
        lr.SetPosition(1, notes[1].transform.position);

        //Set Notes to Hold Start/End NoteType
        notes[0].SetNoteType(notes[0].isStart ? NoteType.Note.holdStart : NoteType.Note.holdEnd);
        notes[1].SetNoteType(notes[1].isStart ? NoteType.Note.holdStart : NoteType.Note.holdEnd);
    }

    public void FillEmptyNotes()
    {
        //Find Empty Note
        Note[] _notes = FindObjectsOfType<Note>();

        for (int i = 0; i < _notes.Length; i++)
        {
            //Insert Empty Note prefab into Chart
            if (!_notes[i].isOccupied)
            {
                //Instantiate
                tmpNote = Instantiate(emptyNote, wholeChart.transform);
                Note _emptyNote = tmpNote.GetComponent<Note>();
                _emptyNote.noteIndex = _notes[i].noteIndex;
                _emptyNote.isInTrack = true;
                _emptyNote.isOccupied = true;
                Debug.Log("Instantiating: " + _notes[i].name);
            }
        }
    }

    //put first note at the track's start position
    public void FinishChart()
    {
        FillEmptyNotes();

        wholeChart.transform.position = tracks[0].endPos;

        //Prepare Chart Component
        wholeChart.GetComponent<Chart>().SetBeats(Convert.ToInt32(inputField.text));


        PrefabUtility.SaveAsPrefabAsset(wholeChart, "Assets/Content/Prefabs/Charts/Chart" + uniChartID + ".prefab");
        uniChartID++;
    }

    public void Play() //when button is pressed, start moving notes
    {
        isPlaying = true;

        wholeChart.transform.position += new Vector3(-noteOffset * Time.deltaTime, 0, 0);
    }

    public void SetNumOfBeats()
    {
        for (int i = 0; i < notePositions.Count; i++)
        {
            Destroy(notePositions[i]);
        }
        notePositions.Clear();

        float trackLenght = Vector3.Distance(tracks[0].startPos, tracks[0].endPos);
        int numberOfBeats = Convert.ToInt32(inputField.text);
        float noteoffset = trackLenght / (numberOfBeats - 1);

        for (int i = 0; i < numberOfBeats; i++)
        {
            notePositions.Add(Instantiate(emptyNote, new Vector3(tracks[0].endPos.x - (noteoffset * i), tracks[0].endPos.y, 0), Quaternion.identity));
            notePositions[i].GetComponent<Note>().noteIndex = i;
        }
    }
}