using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Conductor : MonoBehaviour
{
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //How many seconds until first beat is counted
    public float firstBeatOffset;

    //the number of beats in each loop
    public float beatsPerLoop;

    //the total number of loops completed since the looping clip first started
    public int completedLoops = 0;

    //The current position of the song within the loop in beats.
    public float loopPositionInBeats;

    //The current relative position of the song within the loop measured between 0 and 1.
    public float loopPositionInAnalog;

    public float loopBeatIndexPositionAnalog;
    public int loopBeatIndexPosition;
    private float lastSavedtime;

    //Conductor instance
    public static Conductor instance;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        Events.PhraseEnded += LastSavedTime;
    }

    void OnDisable()
    {
        Events.PhraseEnded -= LastSavedTime;
    }

    void Start()
    {
        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;
    }

    void Update()
    {
        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        //calculate the loop position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;
        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;

        loopBeatIndexPositionAnalog = songPositionInBeats % TrackFactory.instance.ChartToSpawn.notes.Count;
        loopBeatIndexPosition = Mathf.FloorToInt(loopBeatIndexPositionAnalog);
    }

    void LastSavedTime()
    {
        //Debug.Log("Savedtime");
        lastSavedtime = songPositionInBeats;
    }
}
