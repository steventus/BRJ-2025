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
    public GameObject badMessage;

    [Header("Damage")]
    [SerializeField] private int perfectHitDmg = 3;
    [SerializeField] private int hitDmg = 1;

    [Header("Threshold Adjustment")]
    public float minThresholdForNoteHit = 0.4f;
    public float perfectHitThreshold = 0.2f;

    #region Private
    private bool ifHandlingMiss = false;
    #endregion
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

        HandleMissedNotes();
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

    public void InitialiseMissHandling(bool _state)
    {
        ifHandlingMiss = _state;
        oldNote = beatMarkers[0].GetComponent<IPlayerInteractable>();
    }
    public void ClearBeatsList()
    {
        beatMarkers.Clear();
    }

    public HitType CheckIfInputIsOnBeat()
    {
        float inputPressDistanceFromBeat = Mathf.Abs((float)beatIndex - conductor.loopPositionInBeats);

        if (inputPressDistanceFromBeat < perfectHitThreshold)
        {
            return HitType.perfect;
        }
        else if (inputPressDistanceFromBeat < minThresholdForNoteHit)
        {
            return HitType.good;
        }
        else
        {
            return HitType.miss;
        }
    }

    public void PerfectHit()
    {

        perfectMessage.SetActive(true);
        hitMessage.SetActive(false);
        missMessage.SetActive(false);

        Events.OnSuccessfulNoteHit?.Invoke(perfectHitDmg);
        Invoke("DisableMessages", 0.25f);
    }

    public void GoodHit()
    {
        hitMessage.SetActive(true);
        perfectMessage.SetActive(false);
        missMessage.SetActive(false);

        //invoke successful input event
        Events.OnSuccessfulNoteHit?.Invoke(hitDmg);
        Invoke("DisableMessages", 0.25f);
    }

    public void MissHit()
    {
        missMessage.SetActive(true);
        perfectMessage.SetActive(false);
        hitMessage.SetActive(false);

        //invoke failed input event aka miss
        Events.OnUnsuccessfulNoteHit?.Invoke(hitDmg);
        Invoke("DisableMessages", 0.25f);
    }

    public void BadHit()
    {
        badMessage.SetActive(true);

        //invoke bad input event aka. hitting bad notes
        Events.OnBadNoteHit?.Invoke(hitDmg);
        Invoke("DisableMessages", 0.25f);
    }

    void HandleMissedNotes()
    {
        if (oldNote != null && ifHandlingMiss)
        {
            if (oldNote != currentNote)
            {
                oldNote.OnMiss();
                oldNote = currentNote;
            }
        }
    }

    void DisableMessages()
    {
        perfectMessage.SetActive(false);
        hitMessage.SetActive(false);
        missMessage.SetActive(false);
        badMessage.SetActive(false);
    }
}
