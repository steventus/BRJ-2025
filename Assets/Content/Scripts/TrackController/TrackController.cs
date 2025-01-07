using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    [SerializeField] private List<Track> Tracks;
    private Track currentTrack;
    private int index;
    void Start()
    {
        index = 1;
        UpdateTrack();
    }
    public void NextTrack()
    {
        index++;
        if (index >= Tracks.Count)
            index = 0;

        UpdateTrack();
    }

    public void PreviousTrack()
    {
        index--;
        if (index <= 0)
            index = Tracks.Count - 1;

        UpdateTrack();
    }

    private void UpdateTrack()
    {
        currentTrack = Tracks[index];
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(currentTrack.transform.position, Vector3.one * 0.3f);
    }
}
