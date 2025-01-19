using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource currentMusicSource;
    [SerializeField] AudioSource nextMusicSource;
    [SerializeField] float volume = 1;
    [SerializeField] float fadeDuration;
    bool isFadingBetweenSongs = false;
    [Header("Bosses")]
    public Transform bossContainer;
    public List<BossBehaviour> bossBehaviours = new();
    public int currentBossIndex = 0;
    void Awake() {
        //Get and store all Boss objs
        foreach(Transform boss in bossContainer) {
            if(boss.TryGetComponent(out BossBehaviour bossBehaviour)) {
                bossBehaviours.Add(bossBehaviour);
            }
        }
        currentMusicSource = bossBehaviours[0].musicSource; 
    }

    void Start() {
        //set starting music source (first boss)
        currentMusicSource = bossBehaviours[0].musicSource; 

        currentMusicSource.volume = volume;
    }
    
    public void StartFade() {
        currentBossIndex++;
        if(currentBossIndex >= bossBehaviours.Count) {
            currentBossIndex = 0;
        }

        nextMusicSource = bossBehaviours[currentBossIndex].musicSource; 
        
        currentMusicSource.volume = 0;
        nextMusicSource.volume = volume;
        currentMusicSource = nextMusicSource;
    }
}
