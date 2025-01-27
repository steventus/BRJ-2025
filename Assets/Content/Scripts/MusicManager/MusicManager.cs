using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// In the future, this can totally be retrofitted as a statemachine as well with BossMusicBehaviour as the States. But for the sake of putting this together, I'm doing a haphazard approach.
/// - Steventus
/// </summary>
public class MusicManager : MonoBehaviour
{
    [SerializeField] float volume = 1;
    [SerializeField] float fadeDuration;
    bool isFadingBetweenSongs = false;
    [Header("Bosses")]
    public Transform bossContainer;
    public List<BossMusicBehaviour> bossMusicBehaviours = new();
    public int currentBossIndex = 0;
    private BossMusicBehaviour currentMusic;
    private BossMusicBehaviour nextMusic;
    void Awake() {
        //Get and store all Boss objs
        foreach(Transform boss in bossContainer) {
            if(boss.TryGetComponent(out BossMusicBehaviour _bossMusicBehaviour)) {
                _bossMusicBehaviour.SetVolume(volume);

                //Mute all audio sources
                _bossMusicBehaviour.SetMute(true);

                //Initialise all audio sources to prevent lag spikes in game
                _bossMusicBehaviour.PlayAll();

                //Let them all loop, in order for them to play out the intro, use PlayAll method instead of PlayAllAtLoop.
                _bossMusicBehaviour.SetLoop(true);

                bossMusicBehaviours.Add(_bossMusicBehaviour);
            }
        }

        //currentMusicSource = bossMusicBehaviours[0].musicSource; 

    }

    void Start() {
        //set starting music source (first boss)
        currentMusic = bossMusicBehaviours[0]; 

        currentMusic.SetMute(false);
        bossMusicBehaviours[0].PlayAll();
    }

    public void DebugStartAtLoop(){
        currentMusic.PlayAllAtLoop();
    }

    void Update(){
        currentMusic.HandleUpdate();
    }
    
    public void StartFade() {
        currentBossIndex++;
        if(currentBossIndex >= bossMusicBehaviours.Count) {
            currentBossIndex = 0;
        }

        nextMusic = bossMusicBehaviours[currentBossIndex]; 
        
        //currentMusic.SetMute(true);
        //currentMusic.StopAll();
        Metronome.instance.barIndex = 0;
        currentMusic.FadeOutTransition();

        //nextMusic.SetMute(false);
        //nextMusic.PlayAllAtLoop();
        
        //nextMusic.FadeInTransition();

        currentMusic = nextMusic;
    }
}
