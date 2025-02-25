using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BossMusicBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource highDrums, lowDrums, melody, chords, fx, bassline, phase2;
    [SerializeField] private BossMusicScriptable phaseOneMusic, phaseTwoMusic;
    private int loopStartTimeInSamples => phaseOneMusic.timeAtDropOrChorusInSamples;
    private int loopEndTimeInSamples => phaseOneMusic.timeEndInSamples;
    private bool isLooping;
    private int projectSampleRate => phaseOneMusic.projectSampleRate;
    private int projectBeatsPerBar => phaseOneMusic.beatsPerBar;
    public BossMusicScriptable.Genre genre => phaseOneMusic.musicGenre;
    public void PlayAll()
    {
        highDrums.Play();
        lowDrums.Play();
        melody.Play();
        chords.Play();
        fx.Play();
        bassline.Play();
        phase2.Play();
    }

    public void StopAll()
    {
        highDrums.Stop();
        lowDrums.Stop();
        melody.Stop();
        chords.Stop();
        fx.Stop();
        bassline.Stop();
        phase2.Stop();
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
    void Update()
    {
        //Debug.Log(gameObject.name + " high Drums at: " + highDrums.timeSamples);
    }
    public void HandleUpdate()
    {
        //Debug.Log(highDrums.timeSamples);
        //Debug.Log(loopStartTimeInSamples);
        //Debug.Log(loopEndTimeInSamples);
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
        StartCoroutine(CoroScheduleStop(melody, phaseOneMusic.melodyTransitionOutDurInBars));
        StartCoroutine(CoroScheduleStop(chords, phaseOneMusic.chordsTransitionOutDurInBars));
        StartCoroutine(CoroScheduleStop(fx, phaseOneMusic.fxTransitionOutDurInBars));

        //Stop immediately low drums, bassline
        lowDrums.mute = true;
        //Debug.Log("Stopping Low Drums at: " + Conductor.instance.songPosition);

        bassline.mute = true;

        phase2.mute = true;

        Debug.Log("Stopping bassline Drums at: " + Conductor.instance.songPosition);

        //Fade out High Drums beyond transition
        StartCoroutine(CoroScheduleStop(highDrums, phaseOneMusic.highDrumsTransitionOutDurInBars));
    }

    public void ScheduleFadeOutTransition(float _zeroTime)
    {
        //lowDrums.PlayScheduled(_zeroTime);
        //bassline.PlayScheduled(_zeroTime);
        //
        //melody.PlayScheduled(_zeroTime + Count.BarsToSeconds(phaseOneMusic.melodyTransitionOutDurInBars, Conductor.instance.songBpm));
        //chords.PlayScheduled(_zeroTime + Count.BarsToSeconds(phaseOneMusic.chordsTransitionOutDurInBars, Conductor.instance.songBpm));
        //fx.PlayScheduled(_zeroTime + Count.BarsToSeconds(phaseOneMusic.fxTransitionOutDurInBars, Conductor.instance.songBpm));
        //highDrums.PlayScheduled(_zeroTime + Count.BarsToSeconds(phaseOneMusic.highDrumsTransitionOutDurInBars, Conductor.instance.songBpm));
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
        while (_count <= _numberOfBars - 1) //MINUS 1 (-1) instead of normal value due to racing conditions - where the Bar End is just being invoked one bar too late, Not sure why it occurs but this is the fix atm -Steventus
        {
            yield return new WaitForEndOfFrame();
        }
        Events.BarEnded -= _addCount;

        //After PhrasesEnded a number of Phrases aka Bars, run event.
        _source.mute = true;
        Debug.Log("Stopping " + _source.name + " at: " + Conductor.instance.songPosition);


    }
    public void FadeInTransition()
    {
        //PlayScheduled Low Drums, Melody, Chords, Bassline
        //TODO: Play immediately and use timesamples to start at specific points
        StartCoroutine(CoroSchedulePlay(melody, phaseOneMusic.melodyTransitionInDurInBars));
        StartCoroutine(CoroSchedulePlay(chords, phaseOneMusic.chordsTransitionInDurInBars));
        StartCoroutine(CoroSchedulePlay(bassline, phaseOneMusic.basslineTransitionInDurInBars));
        StartCoroutine(CoroSchedulePlay(lowDrums, phaseOneMusic.lowDrumsTransitionInDurInBars));

        //Play immediately high drums, FX
        highDrums.mute = false;
        fx.mute = false;

        if (GetComponent<BossBehaviour>().ifReadyToTransition)
            phase2.mute = false;

        Debug.Log("Playing highdrums and fx: " + Conductor.instance.songPosition);
    }

    public void ScheduleFadeInTrasition(float _zeroTime)
    {
        highDrums.PlayScheduled(_zeroTime);
        fx.PlayScheduled(_zeroTime);


        melody.PlayScheduled(_zeroTime + Count.BarsToSeconds(phaseOneMusic.melodyTransitionInDurInBars, Conductor.instance.songBpm));
        chords.PlayScheduled(_zeroTime + Count.BarsToSeconds(phaseOneMusic.chordsTransitionInDurInBars, Conductor.instance.songBpm));
        bassline.PlayScheduled(_zeroTime + Count.BarsToSeconds(phaseOneMusic.basslineTransitionInDurInBars, Conductor.instance.songBpm));
        lowDrums.PlayScheduled(_zeroTime + Count.BarsToSeconds(phaseOneMusic.lowDrumsTransitionInDurInBars, Conductor.instance.songBpm));

        Debug.Log("scheduled to: " + _zeroTime + ". and melody at: " + (_zeroTime + phaseOneMusic.melodyTransitionInDurInBars, Conductor.instance.songBpm));
    }
    private IEnumerator CoroSchedulePlay(AudioSource _source, int _numberOfBars)
    {
        //Subscribe to PhaseEnded, and count
        int _count = 0;
        UnityAction _addCount = new UnityAction(() =>
        {
            _count++;
        });

        Events.BarEnded += _addCount;
        while (_count <= _numberOfBars - 1) //MINUS 1 (-1) instead of normal value due to racing conditions - where the Bar End is just being invoked one bar too late, Not sure why it occurs but this is the fix atm -Steventus
        {
            yield return new WaitForEndOfFrame();
        }
        Events.BarEnded -= _addCount;

        //After PhrasesEnded a number of Phrases aka Bars, run event.
        _source.mute = false;
        Debug.Log("Playing " + _source.name + " at: " + Conductor.instance.songPosition);
    }

    private void LoopSong()
    {
        highDrums.timeSamples = lowDrums.timeSamples = melody.timeSamples = chords.timeSamples = fx.timeSamples = bassline.timeSamples = phase2.timeSamples = loopStartTimeInSamples;
    }
}
