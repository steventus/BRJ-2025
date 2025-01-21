using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Metronome : MonoBehaviour
{
    public static Metronome instance = null;

    [SerializeField] Conductor conductor;
    [SerializeField] RectTransform metronomeLine;
    [SerializeField] RectTransform beatMarkerParent;
    public List<RectTransform> beatMarkers = new List<RectTransform>();
    int beatIndex;
    public RectTransform nextBeat;
    public IPlayerInteractable currentNote => nextBeat.GetComponent<IPlayerInteractable>();
    private IPlayerInteractable oldNote;

    public float lerpSpeed;

    public enum HitType
    {
        perfect,
        good,
        miss
    }

    public GameObject perfectMessage;
    public GameObject hitMessage;
    public GameObject missMessage;

    public float minThresholdForNoteHit = 0.4f;
    public float perfectHitThreshold = 0.2f;
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }
    void Start()
    {
        beatIndex = 0;
        ClearBeatsList();
    }

    void Update()
    {
        if (beatMarkers.Count == 0)
            return;

        beatIndex = Mathf.Clamp(beatIndex, 0, beatMarkers.Count);
        beatIndex = Mathf.FloorToInt(conductor.loopPositionInBeats);

        if (beatIndex >= beatMarkers.Count)
        {
            nextBeat = beatMarkers[0];
        }
        else
        {
            nextBeat = beatMarkers[beatIndex];
        }

        metronomeLine.anchoredPosition = Vector3.Lerp(metronomeLine.anchoredPosition, nextBeat.anchoredPosition, Time.deltaTime * lerpSpeed);

        //HandleMissedNotes();
    }

    public void SetMarkers()
    {
        ClearBeatsList();

        for (int i = 0; i < TrackFactory.instance.notes.Count; i++)
        {
            RectTransform beat = TrackFactory.instance.notes[i].GetComponent<RectTransform>();
            if (!beatMarkers.Contains(beat))
                beatMarkers.Add(beat);
        }
    }
    public void ClearBeatsList()
    {
        beatMarkers.Clear();
    }
    //public void CheckIfInputIsOnBeat()
    //{
    //    int perfectHitDmg = 3;
    //    int hitDmg = 1;
    //
    //    float inputPressDistanceFromBeat = Mathf.Abs((float)beatIndex - conductor.loopPositionInBeats);
    //    Debug.Log(inputPressDistanceFromBeat);
    //    if (inputPressDistanceFromBeat < perfectHitThreshold)
    //    {
    //        perfectMessage.SetActive(true);
    //        hitMessage.SetActive(false);
    //        missMessage.SetActive(false);
    //
    //        Events.OnSuccessfulNoteHit?.Invoke(perfectHitDmg);
    //    }
    //    else if (inputPressDistanceFromBeat < minThresholdForNoteHit)
    //    {
    //        hitMessage.SetActive(true);
    //        perfectMessage.SetActive(false);
    //        missMessage.SetActive(false);
    //
    //        //invoke successful input event
    //        Events.OnSuccessfulNoteHit?.Invoke(hitDmg);
    //    }
    //    else
    //    {
    //        hitMessage.SetActive(false);
    //        missMessage.SetActive(true);
    //
    //        //invoke failed input event
    //        Events.OnUnsuccessfulNoteHit?.Invoke(hitDmg);
    //    }
    //
    //    Invoke("DisableMessages", 0.25f);
    //}
    public HitType CheckIfInputIsOnBeat()
    {
        int perfectHitDmg = 3;
        int hitDmg = 1;
        float inputPressDistanceFromBeat = Mathf.Abs((float)beatIndex - conductor.loopPositionInBeats);

        Invoke("DisableMessages", 0.25f);

        if (inputPressDistanceFromBeat < perfectHitThreshold)
        {
            perfectMessage.SetActive(true);
            hitMessage.SetActive(false);
            missMessage.SetActive(false);

            Events.OnSuccessfulNoteHit?.Invoke(perfectHitDmg);
            return HitType.perfect;
        }
        else if (inputPressDistanceFromBeat < minThresholdForNoteHit)
        {
            hitMessage.SetActive(true);
            perfectMessage.SetActive(false);
            missMessage.SetActive(false);

            //invoke successful input event
            Events.OnSuccessfulNoteHit?.Invoke(hitDmg);
            return HitType.good;
        }
        else
        {
            hitMessage.SetActive(false);
            missMessage.SetActive(true);

            //invoke failed input event
            Events.OnUnsuccessfulNoteHit?.Invoke(hitDmg);
            return HitType.miss;
        }
    }

    void HandleMissedNotes()
    {
        oldNote.OnMiss();
    }

    void DisableMessages()
    {
        perfectMessage.SetActive(false);
        hitMessage.SetActive(false);
        missMessage.SetActive(false);
    }
}
