using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    private Track[] tracks;
    List<GameObject> activeNotes = new List<GameObject>();


    public float noteVel;

    private void Start()
    {
        tracks = GameObject.FindObjectsOfType<Track>();

        SpawnNote(1);
    }

    private void Update()
    {
        MoveAllNotes();
    }

    void SpawnNote(int tracknum)
    {
        activeNotes.Add(Instantiate(notePrefab, tracks[tracknum].startPos, Quaternion.identity));
    }

    void MoveAllNotes()
    {
        for(int i = 0;  i < activeNotes.Count; i++) 
        {
            activeNotes[i].transform.position += new Vector3 (noteVel * Time.deltaTime, 0, 0);
        }
    }
}
