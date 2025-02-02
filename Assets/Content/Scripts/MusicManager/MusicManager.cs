using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// In the future, this can totally be retrofitted as a statemachine as well with BossMusicBehaviour as the States. But for the sake of putting this together, I'm doing a haphazard approach.
/// - Steventus
/// </summary>

public class DrumFill
{
    public enum Transition
    {
        //Drum fills are sorted according to how bosses are stored in containers (aka. how they are arranged in hierarchy)
        electroSwingToDigital,
        electroSwingToFunk,
        digitalToFunk
    }
    public Transition genreTransition;
    public AudioClip drumfillAudioClip;

    public DrumFill(Transition _genreTransition, AudioClip _audioClip)
    {
        genreTransition = _genreTransition;
        drumfillAudioClip = _audioClip;
    }
}
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance = null;
    [SerializeField] float volume = 1;
    [SerializeField] float fadeDuration;
    bool isFadingBetweenSongs = false;
    [Header("Bosses")]
    [SerializeField] private Transform bossContainer;

    public BossMusicBehaviour[] bossMusicBehavioursss;

    private List<BossMusicBehaviour> bossMusicBehaviours = new();
    private int currentBossIndex = 0;

    [Header("Transitions")]
    [SerializeField] private AudioSource drumfillSource;
    [SerializeField] private AudioClip electroSwingToFunk, electroSwingToDigital, digitalToFunk;
    private List<DrumFill> drumFills;
    private BossMusicBehaviour currentMusic;
    private BossMusicBehaviour nextMusic;
    private bool ifScheduled = false;
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    public void StartMusic()
    {
        //Get and store all Boss objs - let them all play simultaneously to make sure they're all in sync
        
        
        /*foreach (Transform boss in bossContainer)
        {
            if (boss.TryGetComponent(out BossMusicBehaviour _bossMusicBehaviour))
            {
                _bossMusicBehaviour.SetVolume(volume);

                //Initialise all audio sources to prevent lag spikes in game
                _bossMusicBehaviour.PlayAll();
                
                //Mute all audio sources
                _bossMusicBehaviour.SetMute(true);

                //Let them all loop, in order for them to play out the intro, use PlayAll method instead of PlayAllAtLoop.
                _bossMusicBehaviour.SetLoop(true);

                bossMusicBehaviours.Add(_bossMusicBehaviour);

                Debug.Log("StartTime for:" + _bossMusicBehaviour.name + " at " + Conductor.instance.songPosition);
            }
        }*/

        foreach (BossMusicBehaviour BMB in bossMusicBehavioursss)
        {
            BMB.SetVolume(volume);
            BMB.PlayAll();
            BMB.SetMute(true);
            BMB.SetLoop(true);

            bossMusicBehaviours.Add(BMB);
        }

        drumFills = new List<DrumFill>
        {
            new DrumFill(DrumFill.Transition.electroSwingToDigital, electroSwingToDigital),
            new DrumFill(DrumFill.Transition.electroSwingToFunk, electroSwingToFunk),
            new DrumFill(DrumFill.Transition.digitalToFunk, digitalToFunk)
        };

        drumfillSource.Play();
        drumfillSource.mute = true;

        //set starting music source (first boss)
        currentMusic = bossMusicBehaviours[0];

        currentMusic.SetMute(false);
    }

    public void DebugStartAtLoop()
    {
        currentMusic.PlayAllAtLoop();
    }

    void Update()
    {
        for (int i = 0; i < bossMusicBehaviours.Count; i++)
        {
            bossMusicBehaviours[i].HandleUpdate();
        }
    }

    public void StartFade()
    {
        currentBossIndex++;
        if (currentBossIndex >= bossMusicBehaviours.Count)
        {
            currentBossIndex = 0;
        }

        nextMusic = bossMusicBehaviours[currentBossIndex];

        Metronome.instance.barIndex = 0;

        currentMusic.FadeOutTransition();
        //PlayTransitionFill();
        nextMusic.FadeInTransition();

        currentMusic = nextMusic;
    }

    public void ScheduleFade(float timeToRun)
    {
        //Check only run once
        if (ifScheduled)
            return;

        ifScheduled = true;

        //Fade out etc all from time to run
        currentBossIndex++;
        if (currentBossIndex >= bossMusicBehaviours.Count)
        {
            currentBossIndex = 0;
        }

        nextMusic = bossMusicBehaviours[currentBossIndex];

        //Apply appropriate scheduling here
        currentMusic.FadeOutTransition();
        //ScheduleTransitionFill(timeToRun);
        nextMusic.FadeInTransition();

        currentMusic = nextMusic;
    }

    public void PlayTransitionFill()
    {
        DrumFill.Transition _genreTransition = DrumFill.Transition.electroSwingToFunk;

        switch (currentMusic.genre)
        {
            case BossMusicScriptable.Genre.ElectroSwing:
                if (nextMusic.genre == BossMusicScriptable.Genre.Digital) _genreTransition = DrumFill.Transition.electroSwingToDigital;
                else if (nextMusic.genre == BossMusicScriptable.Genre.Funk) _genreTransition = DrumFill.Transition.electroSwingToFunk;
                break;

            case BossMusicScriptable.Genre.Digital:
                if (nextMusic.genre == BossMusicScriptable.Genre.ElectroSwing) _genreTransition = DrumFill.Transition.electroSwingToDigital;
                else if (nextMusic.genre == BossMusicScriptable.Genre.Funk) _genreTransition = DrumFill.Transition.digitalToFunk;
                break;

            case BossMusicScriptable.Genre.Funk:
                if (nextMusic.genre == BossMusicScriptable.Genre.ElectroSwing) _genreTransition = DrumFill.Transition.electroSwingToFunk;
                else if (nextMusic.genre == BossMusicScriptable.Genre.Digital) _genreTransition = DrumFill.Transition.digitalToFunk;
                break;
        }

        DrumFill _drumFillSelection = drumFills.FirstOrDefault(x => x.genreTransition == _genreTransition);

        Debug.Log(_drumFillSelection.genreTransition);

        drumfillSource.clip = _drumFillSelection.drumfillAudioClip;
        drumfillSource.mute = false;
        drumfillSource.Play();
        Debug.Log("Playing " + drumfillSource.name + " at: " + Conductor.instance.songPosition);
    }

    public void ScheduleTransitionFill(float _time)
    {
        DrumFill.Transition _genreTransition = DrumFill.Transition.electroSwingToFunk;

        switch (currentMusic.genre)
        {
            case BossMusicScriptable.Genre.ElectroSwing:
                if (nextMusic.genre == BossMusicScriptable.Genre.Digital) _genreTransition = DrumFill.Transition.electroSwingToDigital;
                else if (nextMusic.genre == BossMusicScriptable.Genre.Funk) _genreTransition = DrumFill.Transition.electroSwingToFunk;
                break;

            case BossMusicScriptable.Genre.Digital:
                if (nextMusic.genre == BossMusicScriptable.Genre.ElectroSwing) _genreTransition = DrumFill.Transition.electroSwingToDigital;
                else if (nextMusic.genre == BossMusicScriptable.Genre.Funk) _genreTransition = DrumFill.Transition.digitalToFunk;
                break;

            case BossMusicScriptable.Genre.Funk:
                if (nextMusic.genre == BossMusicScriptable.Genre.ElectroSwing) _genreTransition = DrumFill.Transition.electroSwingToFunk;
                else if (nextMusic.genre == BossMusicScriptable.Genre.Digital) _genreTransition = DrumFill.Transition.digitalToFunk;
                break;
        }

        DrumFill _drumFillSelection = drumFills.FirstOrDefault(x => x.genreTransition == _genreTransition);

        Debug.Log(_drumFillSelection.genreTransition);

        drumfillSource.clip = _drumFillSelection.drumfillAudioClip;
        drumfillSource.mute = false;
        drumfillSource.PlayScheduled(_time);
        Debug.Log("Playing " + drumfillSource.name + " at: " + _time);
    }

    //STOP ALL MUSIC ON GAME COMPLETE
    public void StopBossMusic()
    {
        bossMusicBehaviours[0].StopAll();
    }
}
