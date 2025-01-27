using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Count{
    public static float BeatsToSeconds(int _numberOfBeats){
        return 60/Conductor.instance.songBpm * _numberOfBeats;
    }
}
[CreateAssetMenu(fileName = "BossMusic", menuName = "ScriptableObjects/New Boss Music", order = 1)]
public class BossMusicScriptable : ScriptableObject
{
    public AudioClip highDrums, lowDrums, melody, chords, fx, bassline;
    public float timeAtDropOrChorusInSeconds => Count.BeatsToSeconds(24);

    //public void PlayAll(){
    //    highDrums.Play();
    //    lowDrums.Play();
    //    melody.Play();
    //    chords.Play();
    //    fx.Play();
    //    bassline.Play();
    //}
//
    //public void FadeOutTransition(){
    //    //Stop Scheduled melody, chords, fx
    //    #region Test
    //    melody.Stop();
    //    chords.Stop();
    //    fx.Stop();
    //    #endregion
//
    //    //Stop immediately low drums, bassline
    //    lowDrums.Stop();
    //    bassline.Stop();
    //    
    //    //Fade out High Drums beyond transition
    //    #region Test
    //    highDrums.Stop();
    //    #endregion
    //}
//
    //public void FadeInTransition(){
    //    //PlayScheduled Low Drums, Melody, Chords, Bassline
    //    lowDrums.SetScheduledStartTime(timeAtDropOrChorusInSeconds);
    //    melody.SetScheduledStartTime(timeAtDropOrChorusInSeconds);
    //    chords.SetScheduledStartTime(timeAtDropOrChorusInSeconds);
    //    bassline.SetScheduledStartTime(timeAtDropOrChorusInSeconds);
//
    //    lowDrums.PlayScheduled(Conductor.instance.songPosition + Count.BeatsToSeconds(8));
    //    melody.PlayScheduled(Conductor.instance.songPosition + Count.BeatsToSeconds(8));
    //    chords.PlayScheduled(Conductor.instance.songPosition + Count.BeatsToSeconds(8));
    //    bassline.PlayScheduled(Conductor.instance.songPosition + Count.BeatsToSeconds(8));
//
    //    //Play immediately high drums, FX
    //    highDrums.Play();
    //    fx.Play();
    //}
}
