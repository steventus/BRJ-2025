using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
                _bossMusicBehaviour.SetMute(true);

                //Initialise all audio sources to prevent lag spikes in game
                _bossMusicBehaviour.PlayAll();

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
    
    public void StartFade() {
        currentBossIndex++;
        if(currentBossIndex >= bossMusicBehaviours.Count) {
            currentBossIndex = 0;
        }

        nextMusic = bossMusicBehaviours[currentBossIndex]; 
        
        currentMusic.SetMute(true);
        currentMusic.StopAll();

        nextMusic.SetMute(false);
        nextMusic.PlayAll();

        currentMusic = nextMusic;
    }
}
