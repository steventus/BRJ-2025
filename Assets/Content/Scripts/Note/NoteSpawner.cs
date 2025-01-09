using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject scratchL, scratchR, holdL, holdR, bossChange, avoid;
    private Track[] tracks;
    List<GameObject> activeNotes = new List<GameObject>();

    public int[] notesPerBeat;
    public int[] trackNum;

    public int[] noteTypes;

    public float noteVel;

    private void Start()
    {
        tracks = GameObject.FindObjectsOfType<Track>();

    }

    private void Update()
    {
        MoveAllNotes();
    }

    void SpawnMusicTrack(float beatsNum, int[] notesPerBeat, int[] trackNum, int[] noteTypes)
    {
        for(int i = 0;  i < beatsNum; i++)
        {
            if (notesPerBeat.Length < i)
            {
                for(int j = 0; j <  notesPerBeat.Length; j++)
                {
                    activeNotes.Add(Instantiate(scratchL, tracks[trackNum[i]].startPos, Quaternion.identity));
                }
            }
        }
    }

    void MoveAllNotes()
    {
        for(int i = 0;  i < activeNotes.Count; i++) 
        {
            activeNotes[i].transform.position += new Vector3 (noteVel * Time.deltaTime, 0, 0);
        }
    }
}
