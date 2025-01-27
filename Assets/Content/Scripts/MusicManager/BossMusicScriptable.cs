using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Count
{
    public static float BeatsToSeconds(int _numberOfBeats)
    {
        return 60 / Conductor.instance.songBpm * _numberOfBeats;
    }
}
[CreateAssetMenu(fileName = "BossMusic", menuName = "ScriptableObjects/New Boss Music", order = 1)]
public class BossMusicScriptable : ScriptableObject
{
    public AudioClip highDrums, lowDrums, melody, chords, fx, bassline;
    [SerializeField] private int projectSampleRate = 44100; //Confirm with Andreas
    public int timeAtDropOrChorusInSeconds = 45; //Andreas counts each bar as 4 beats. Drop/Chorus starts at Bar 25 (or the end of bar 24)
    public int loopEndTimeInSeconds = 105; //Need to confirm with Andreas
    public int timeAtDropOrChorusInSamples => projectSampleRate * timeAtDropOrChorusInSeconds; 
    public int timeEndInSamples => projectSampleRate * loopEndTimeInSeconds; 
}
