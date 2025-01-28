using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Count
{
    public static float BeatsToSeconds(int _numberOfBeats)
    {
        return 60 / Conductor.instance.songBpm * _numberOfBeats;
    }
    public static int BarsToSamples(int ProjectSampleRate, int _numberOfBars){
        return ProjectSampleRate * _numberOfBars;
    }
    public static float BarsToSeconds(int _numberOfBars, float songBpm){
        return _numberOfBars * songBpm/60;
    }
}
[CreateAssetMenu(fileName = "BossMusic", menuName = "ScriptableObjects/New Boss Music", order = 1)]
public class BossMusicScriptable : ScriptableObject
{
    [Header("Project Stats")]
    public int projectSampleRate = 44100; //Confirm with Andreas
    public int timeAtDropOrChorusInSeconds = 45; //Andreas counts each bar as 4 beats. Drop/Chorus starts at Bar 25 (or the end of bar 24)
    public int loopEndTimeInSeconds = 105; //Need to confirm with Andreas
    public int timeAtDropOrChorusInSamples => projectSampleRate * timeAtDropOrChorusInSeconds;
    public int timeEndInSamples => projectSampleRate * loopEndTimeInSeconds;
    public int beatsPerBar = 4;
    public enum Genre {
        ElectroSwing,
        Digital,
        Funk
    }
    public Genre musicGenre;

    /// <summary>
    /// 0 means instantaneous upon called from the moment the Transition Chart is LOADED in from ENEMY TURN
    /// </summary>
    [Header("Stems Transition Out Data")]
    public int highDrumsTransitionOutDurInBars = 4;
    public int lowDrumsTransitionOutDurInBars = 0;
    public int melodyTransitionOutDurInBars = 2;
    public int chordsTransitionOutDurInBars = 2;
    public int fxTransitionOutDurInBars = 2;
    public int basslineTransitionOutDurInBars = 0;

    [Header("Stems Transition In Data")]
    public int highDrumsTransitionInDurInBars = 0;
    public int lowDrumsTransitionInDurInBars = 2;
    public int melodyTransitionInDurInBars = 2;
    public int chordsTransitionInDurInBars = 2;
    public int fxTransitionInDurInBars = 0;
    public int basslineTransitionInDurInBars = 2;
}
