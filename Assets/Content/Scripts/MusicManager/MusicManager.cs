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
    [SerializeField] Slider pitchSlider, volumeSlider, source1VolSlider, source2VolSlider;
    [SerializeField] TextMeshProUGUI songNameDisplay, manualFadeStateDisplay;
    bool isFadingBetweenSongs = false;
    [SerializeField] bool fadeManually = false;
    void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Debug.Log("multiple instance of " + name + " singleton found, destroying duplicate");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }
    void OnEnable() {
        //subscribe sliders to functions
        pitchSlider.onValueChanged.AddListener(ChangePitchSpeed);
        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        source1VolSlider.onValueChanged.AddListener(FirstSourceChangeVolume);
        source2VolSlider.onValueChanged.AddListener(SecondSourceChangeVolume);
    }
    void OnDisable() {

    }
    void Start() {
        currentMusicSource = musicSource1;

        musicSource2.volume = 0;
    }
    void Update()
    {
        ControlTempo();
        ControlVolume();
        ControlSliders();
        //display text
        songNameDisplay.text = "Now Playing: " + "'" + currentMusicSource.clip.name + "'";
        manualFadeStateDisplay.text = "Enabled: " + fadeManually;
    }

    void ControlTempo() {
        musicSource1.pitch = pitchSpeed;
        musicSource2.pitch = pitchSpeed;
    }
    void ControlVolume() {
        if(!isFadingBetweenSongs && !fadeManually)
            currentMusicSource.volume = volume;

    }
    void ControlSliders() {
        if(fadeManually) {
            volumeSlider.interactable = false;

            source1VolSlider.interactable = true;
            source2VolSlider.interactable = true;
        }
        else {
            volumeSlider.interactable = true;

            source1VolSlider.interactable = false;
            source2VolSlider.interactable = false;
        }
    }
    void ChangePitchSpeed(float newSpeed) {
        pitchSpeed = newSpeed;
    }
    void ChangeVolume(float newVolume) {
        volume = newVolume;
    }

    public void ChangeAudioClip(AudioClip newClip) {
        currentMusicSource.Stop();
        currentMusicSource.clip = newClip;
        currentMusicSource.Play();
    }

    public void StartFade() {
        if(fadeManually) return;

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

    void FirstSourceChangeVolume(float value) {
        if(fadeManually)
            musicSource1.volume = value;
    }
    void SecondSourceChangeVolume(float value) {
        if(fadeManually)
            musicSource2.volume = value;
    }

    public void EnableManualFade() {
        fadeManually = !fadeManually;
    }
}
