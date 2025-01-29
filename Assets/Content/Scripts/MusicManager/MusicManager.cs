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
    private List<BossMusicBehaviour> bossMusicBehaviours = new();
    private int currentBossIndex = 0;

    [Header("Transitions")]
    [SerializeField] private AudioSource drumfillSource;
    [SerializeField] private AudioClip electroSwingToFunk, electroSwingToDigital, digitalToFunk;
    private List<DrumFill> drumFills;
    private BossMusicBehaviour currentMusic;
    private BossMusicBehaviour nextMusic;
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        //Get and store all Boss objs
        foreach (Transform boss in bossContainer)
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
            }
        }

        drumFills = new List<DrumFill>
        {
            new DrumFill(DrumFill.Transition.electroSwingToDigital, electroSwingToDigital),
            new DrumFill(DrumFill.Transition.electroSwingToFunk, electroSwingToFunk),
            new DrumFill(DrumFill.Transition.digitalToFunk, digitalToFunk)
        };

        drumfillSource.Play();
        drumfillSource.mute = true;
    }

    public void StartMusic()
    {
        //set starting music source (first boss)
        currentMusic = bossMusicBehaviours[0];

        currentMusic.SetMute(false);
        bossMusicBehaviours[0].PlayAll();
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
        PlayTransitionFill();
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
        drumfillSource.timeSamples = 0;
        drumfillSource.mute = false;
    }

    //STOP ALL MUSIC ON GAME COMPLETE
    public void StopBossMusic() {
        bossMusicBehaviours[0].StopAll();
    }
}
