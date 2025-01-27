using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource highDrums, lowDrums, melody, chords, fx, bassline;
    [SerializeField] private BossMusicScriptable phaseOneMusic, phaseTwoMusic;
    private float chorusTimeInSecs => phaseOneMusic.timeAtDropOrChorusInSeconds;
    private int loopStartTimeInSamples => phaseOneMusic.timeAtDropOrChorusInSamples;
    private int loopEndTimeInSamples => phaseOneMusic.timeEndInSamples;
    private bool isLooping;
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
    public void HandleUpdate()
    {
        //if (!isLooping)
        //    return;

        if (highDrums.timeSamples >= loopEndTimeInSamples)
        {
            highDrums.timeSamples = lowDrums.timeSamples = melody.timeSamples = chords.timeSamples = fx.timeSamples = bassline.timeSamples = loopStartTimeInSamples;
        }

        Debug.Log("HighDrums: " + highDrums.timeSamples);
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
        #region Test
        melody.Stop();
        chords.Stop();
        fx.Stop();
        #endregion

        //Stop immediately low drums, bassline
        lowDrums.Stop();
        bassline.Stop();

        //Fade out High Drums beyond transition
        #region Test
        highDrums.Stop();
        #endregion
    }

    public void FadeInTransition()
    {
        //PlayScheduled Low Drums, Melody, Chords, Bassline
        lowDrums.SetScheduledStartTime(chorusTimeInSecs);
        melody.SetScheduledStartTime(chorusTimeInSecs);
        chords.SetScheduledStartTime(chorusTimeInSecs);
        bassline.SetScheduledStartTime(chorusTimeInSecs);

        lowDrums.PlayScheduled(Conductor.instance.songPosition + Count.BeatsToSeconds(8));
        melody.PlayScheduled(Conductor.instance.songPosition + Count.BeatsToSeconds(8));
        chords.PlayScheduled(Conductor.instance.songPosition + Count.BeatsToSeconds(8));
        bassline.PlayScheduled(Conductor.instance.songPosition + Count.BeatsToSeconds(8));

        //Play immediately high drums, FX
        highDrums.Play();
        fx.Play();
    }
}
