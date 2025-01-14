using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource currentMusicSource;
    [SerializeField] AudioSource nextMusicSource;
    float volume = 1;
    [SerializeField] float fadeDuration;
    bool isFadingBetweenSongs = false;
    bool hasStarted = false;
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
    }
    
    public void StartFade() {
        if(!hasStarted) {
            //do not fade between audioSources
            currentMusicSource.volume = 1;
            hasStarted = true;
        }
        else {
            currentBossIndex++;
            if(currentBossIndex >= bossBehaviours.Count) {
                currentBossIndex = 0;
            }

            nextMusicSource = bossBehaviours[currentBossIndex].musicSource; 

            StartCoroutine(FadeBetweenSongs(currentMusicSource, nextMusicSource));
        }
    }

    IEnumerator FadeBetweenSongs(AudioSource fadeOut, AudioSource fadeIn) {
        float timeElapsed = 0;

        isFadingBetweenSongs = true;
        fadeOut.volume = volume;
        fadeIn.volume = 0;

        while(timeElapsed < fadeDuration) {
            fadeOut.volume = Mathf.Lerp(volume, 0, timeElapsed / fadeDuration);
            fadeIn.volume = Mathf.Lerp(0, volume, timeElapsed / fadeDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        currentMusicSource = fadeIn;
        isFadingBetweenSongs = false;
    }
}
