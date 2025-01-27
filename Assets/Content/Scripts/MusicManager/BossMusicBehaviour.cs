using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BossMusicBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource highDrums, lowDrums, melody, chords, fx, bassline;
    [SerializeField] private BossMusicScriptable phaseOneMusic, phaseTwoMusic;
    private float chorusTimeInSecs => phaseOneMusic.timeAtDropOrChorusInSeconds;
    private int loopStartTimeInSamples => phaseOneMusic.timeAtDropOrChorusInSamples;
    private int loopEndTimeInSamples => phaseOneMusic.timeEndInSamples;
    private bool isLooping;
    private int projectSampleRate => phaseOneMusic.projectSampleRate;
    private int projectBeatsPerBar => phaseOneMusic.beatsPerBar;
    public void PlayAll()
    {
        highDrums.Play();
        lowDrums.Play();
        melody.Play();
        chords.Play();
        fx.Play();
        bassline.Play();
    }

    public void StopAll()
    {
        highDrums.Stop();
        lowDrums.Stop();
        melody.Stop();
        chords.Stop();
        fx.Stop();
        bassline.Stop();
    }

    //Call when transitioning songs to immediately start at loop section
    public void PlayAllAtLoop()
    {
        PlayAll();
        highDrums.timeSamples = lowDrums.timeSamples = melody.timeSamples = chords.timeSamples = fx.timeSamples = bassline.timeSamples = loopStartTimeInSamples;
    }

    private void PlayAtSample(AudioSource _source, int sample)
    {
        _source.timeSamples = sample;
    }
    public void HandleUpdate()
    {
        if (isLooping && highDrums.timeSamples >= loopEndTimeInSamples)
        {
            LoopSong();
        }
    }

    public void SetVolume(float _volume)
    {
        highDrums.volume = lowDrums.volume = melody.volume = chords.volume = fx.volume = bassline.volume = _volume;
    }

    public void SetMute(bool _ifTrue)
    {
        highDrums.mute = lowDrums.mute = melody.mute = chords.mute = fx.mute = bassline.mute = _ifTrue;
    }

    public void SetLoop(bool _state)
    {
        isLooping = _state;
    }

    public void FadeOutTransition()
    {
        //Stop Scheduled melody, chords, fx
        //StartCoroutine(ScheduleStop(melody, Count.BarsToSamples(projectSampleRate, phaseOneMusic.melodyTransitionOutDurInBars)));
        //StartCoroutine(ScheduleStop(chords, Count.BarsToSamples(projectSampleRate, phaseOneMusic.chordsTransitionOutDurInBars)));
        //StartCoroutine(ScheduleStop(fx, Count.BarsToSamples(projectSampleRate, phaseOneMusic.fxTransitionOutDurInBars)));
        StartCoroutine(CoroScheduleStop(melody, phaseOneMusic.melodyTransitionOutDurInBars));
        StartCoroutine(CoroScheduleStop(chords, phaseOneMusic.chordsTransitionOutDurInBars));
        StartCoroutine(CoroScheduleStop(fx, phaseOneMusic.fxTransitionOutDurInBars));

        //Stop immediately low drums, bassline
        lowDrums.Stop();
        bassline.Stop();

        //Fade out High Drums beyond transition
        #region TODO: Schedule it to fade over time instead of smash cut
        //StartCoroutine(ScheduleStop(highDrums, Count.BarsToSamples(projectSampleRate, phaseOneMusic.highDrumsTransitionOutDurInBars)));
        StartCoroutine(CoroScheduleStop(highDrums, phaseOneMusic.highDrumsTransitionOutDurInBars));

        #endregion
    }

    private IEnumerator CoroScheduleStop(AudioSource _source, int _numberOfBars)
    {
        //Subscribe to PhaseEnded, and count
        int _count = 0; 
        UnityAction _addCount = new UnityAction(() =>
        {
            _count++;
        });

        Events.BarEnded += _addCount;
        while (_count <= _numberOfBars-1) //MINUS 1 (-1) instead of normal value due to racing conditions - where the Bar End is just being invoked one bar too late, Not sure why it occurs but this is the fix atm -Steventus
        {
            yield return new WaitForEndOfFrame();
        }
        Events.BarEnded -= _addCount;

        //After PhrasesEnded a number of Phrases aka Bars, run event.
        _source.Stop();

    }



    public void FadeInTransition()
    {
        //PlayScheduled Low Drums, Melody, Chords, Bassline
        //TODO: Play immediately and use timesamples to start at specific points
        lowDrums.SetScheduledStartTime(chorusTimeInSecs);
        melody.SetScheduledStartTime(chorusTimeInSecs);
        chords.SetScheduledStartTime(chorusTimeInSecs);
        bassline.SetScheduledStartTime(chorusTimeInSecs);

        lowDrums.PlayScheduled(Conductor.instance.songPosition + Count.BeatsToSeconds(8));
        melody.PlayScheduled(Conductor.instance.songPosition + Count.BeatsToSeconds(8));
        chords.PlayScheduled(Conductor.instance.songPosition + Count.BeatsToSeconds(8));
        bassline.PlayScheduled(Conductor.instance.songPosition + Count.BeatsToSeconds(8));

        //Play immediately high drums, FX
        PlayAtSample(highDrums, loopStartTimeInSamples - Count.BarsToSamples(projectSampleRate, phaseOneMusic.highDrumsTransitionInDurInBars));
        PlayAtSample(fx, loopStartTimeInSamples - Count.BarsToSamples(projectSampleRate, phaseOneMusic.fxTransitionInDurInBars));
    }

    private void LoopSong()
    {
        highDrums.timeSamples = lowDrums.timeSamples = melody.timeSamples = chords.timeSamples = fx.timeSamples = bassline.timeSamples = loopStartTimeInSamples;
    }
}
