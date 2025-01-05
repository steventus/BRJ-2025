using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] AudioSource musicSource1, musicSource2;
    [SerializeField] AudioSource currentMusicSource;
    [SerializeField] float pitchSpeed;
    float volume = 1;
    [SerializeField] float fadeDuration;
    [SerializeField] Slider pitchSlider, volumeSlider;
    [SerializeField] TextMeshProUGUI songNameDisplay;
    bool isFadingBetweenSongs = false;
    void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Debug.Log("multiple instance of " + name + " singleton found, destroying duplicate");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        //subscribe pitch function to slider
        pitchSlider.onValueChanged.AddListener(ChangePitchSpeed);
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }
    void Start() {
        currentMusicSource = musicSource1;

        musicSource2.volume = 0;
    }
    void Update()
    {
        ControlTempo();
        ControlVolume();
        DisplayCurrentSongName();
    }

    void ControlTempo() {
        musicSource1.pitch = pitchSpeed;
        musicSource2.pitch = pitchSpeed;
    }
    void ControlVolume() {
        if(!isFadingBetweenSongs)
            currentMusicSource.volume = volume;
    }
    public void ChangePitchSpeed(float newSpeed) {
        pitchSpeed = newSpeed;
    }
    public void ChangeVolume(float newVolume) {
        volume = newVolume;
    }

    public void ChangeAudioClip(AudioClip newClip) {
        currentMusicSource.Stop();
        currentMusicSource.clip = newClip;
        currentMusicSource.Play();
    }

    public void StartFade() {
        if(currentMusicSource == musicSource1) {
            StartCoroutine(FadeBetweenSongs(musicSource1, musicSource2));
        }
        else if (currentMusicSource == musicSource2) {
            StartCoroutine(FadeBetweenSongs(musicSource2, musicSource1));
        }                
    }

    IEnumerator FadeBetweenSongs(AudioSource fadeOut, AudioSource fadeIn) {
        float timeElapsed = 0;

        if(!musicSource1.isPlaying) {
            musicSource1.Play();
        }
        if(!musicSource2.isPlaying) {
            musicSource2.Play();
        }

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

    public void PlayMusic() {
        musicSource1.Play();
        musicSource2.Play();
    }
    public void StopMusic() {
        musicSource2.Stop();
        musicSource1.Stop();
    }

    void DisplayCurrentSongName() {
        songNameDisplay.text = "Now Playing: " + "'" + currentMusicSource.clip.name + "'";
    }
}
