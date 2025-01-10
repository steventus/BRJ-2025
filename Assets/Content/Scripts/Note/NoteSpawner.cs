using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject scratchL, scratchR, bossChange, avoid;
    private Track[] tracks;
    List<GameObject> activeNotes = new List<GameObject>();
    public LayerMask whatIsNote;

    public int[] notesPerBeat;
    public int[] trackNum;

    public int[] noteTypes;

    public float noteOffset;

    private RaycastHit2D hit;
    private Vector3 mousePosRaw;
    private Vector2 mousePos;

    GameObject tmpNote = null;

    GameObject wholeChart;

    bool isPlaying;

    private bool hasPickedUpNote = false;
    private void Start()
    {
        wholeChart = new GameObject();
        tracks = GameObject.FindObjectsOfType<Track>();
        wholeChart.transform.position = tracks[1].startPos;
    }

    private void Update()
    {
        mousePosRaw = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePosRaw.x, mousePosRaw.y, 0f);    

        if (Input.GetMouseButton(0) && Physics2D.Raycast(mousePosRaw, Vector3.zero, Mathf.Infinity, whatIsNote))
        {
            hit = Physics2D.Raycast(mousePosRaw, Vector3.zero, Mathf.Infinity, whatIsNote);
            DragNote();
        }
        if(Input.GetMouseButtonUp(0) && Physics2D.Raycast(mousePosRaw, Vector3.zero, Mathf.Infinity, whatIsNote) && hasPickedUpNote)
        {
            hasPickedUpNote = false;
            CheckClosestTrack(tmpNote);
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveAllNotes(1);
        }
        if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveAllNotes(-1);
        }

        if (isPlaying)
        {
            Play();
        }

        CheckIfTrackIsOccupied();
    }

    //used when building the chart
    void MoveAllNotes(int dir)
    {
        wholeChart.transform.position += new Vector3(noteOffset * dir, 0 ,0);
    }

    //gotta work on this one(fking notes don't follow me if i move too fast lol)
    void DragNote()
    {
        if (!hasPickedUpNote)
        {
            tmpNote = Instantiate(hit.transform.gameObject, mousePos, Quaternion.identity);
            hit.transform.GetComponent<Note>().isPicked = true;
            hasPickedUpNote = true;
            activeNotes.Add(tmpNote);
        }
        if (hasPickedUpNote)
        {
            tmpNote.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
        }    
    }

    //checks the closest track to the picked note
    void CheckClosestTrack(GameObject note)
    {
        float closestDistance = Mathf.Infinity;
        int closestTrackID = -1;

        for(int i = 0; i < tracks.Length; i++)
        {
            if (Vector3.Distance(tracks[i].transform.position, note.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(tracks[i].transform.position, note.transform.position);
                closestTrackID = i;
            }
        }
        if (!tracks[closestTrackID].isOccupied)
        {
            note.transform.position = tracks[closestTrackID].startPos;
            note.transform.parent = wholeChart.transform;
            note.GetComponent<Note>().isPicked = false;
            tracks[closestTrackID].isOccupied = true;
        }
        else
        {
            Destroy(note);
            Debug.Log("Track Already Occupied");
        }
    }

    void CheckIfTrackIsOccupied()
    {
        RaycastHit2D hit;
        

        for (int i = 0; i<tracks.Length; i++)
        {
            hit = Physics2D.Raycast(tracks[i].startPos, Vector3.zero, Mathf.Infinity, whatIsNote);

            Debug.Log(tracks[i].isOccupied);

            if (hit && !hit.transform.GetComponent<Note>().isPicked)
                tracks[i].isOccupied = true;
            else if (hit && hit.transform.GetComponent<Note>().isPicked)
                tracks[i].isOccupied = false;
            else if (!hit)
                tracks[i].isOccupied = false;
        }
    }

    //put first note at the track's start position
    public void FinishChart()
    {
        wholeChart.transform.position = tracks[1].startPos;
    }

    public void Play() //when button is pressed, start moving notes
    {
        isPlaying = true;

        wholeChart.transform.position += new Vector3(noteOffset * Time.deltaTime, 0, 0);
    }
}