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
        UpdateTrack();
    }

    public void PreviousTrack()
    {
        index--;
        UpdateTrack();
    }

    private void UpdateTrack()
    {
        index = Mathf.Clamp(index, 0, Tracks.Count - 1);
        currentTrack = Tracks[index];
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (currentTrack != null)
            Gizmos.DrawWireCube(currentTrack.transform.position, Vector3.one * 0.3f);
    }
}
