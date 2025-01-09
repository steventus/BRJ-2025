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


    private bool hasPickedUpNote = false;
    private void Start()
    {
        tracks = GameObject.FindObjectsOfType<Track>();

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
            MoveAllNotes();
        }
    }

    void MoveAllNotes()
    {
        for(int i = 0;  i < activeNotes.Count; i++) 
        {
            activeNotes[i].transform.position += new Vector3 (noteOffset, 0, 0);
        }
        for(int i = 0; i < tracks.Length; i++)
        {
            tracks[i].isOccupied = false;
        }
    }

    //gotta work on this one(fking notes don't follow me if i move too fast lol)
    void DragNote()
    {
        if (!hasPickedUpNote)
        {
            tmpNote = Instantiate(hit.transform.gameObject, mousePos, Quaternion.identity);
            hasPickedUpNote = true;
            activeNotes.Add(tmpNote);
        }
        if (hasPickedUpNote)
        {
            tmpNote.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
        }    
    }

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
            tracks[closestTrackID].isOccupied = true;
        }
        else
        {
            Destroy(note);
            Debug.Log("Track Already Occupied");
        }
        
    }
}
