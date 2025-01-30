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
    int oldBeatIndex;
    public int barIndex;
    public RectTransform currentBeat;
    public RectTransform nextBeat
    {
        get
        {
            int _selection = beatIndex + 1;
            if (_selection >= beatMarkers.Count)
                _selection = 0;
            return beatMarkers[_selection];
        }
    }
    public IPlayerInteractable currentNote;
    public int beatIndexSelector;
    public GameObject debugCurrentNote;
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
    public float goodHitThreshold = 0.4f;
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

        beatIndex = conductor.loopBeatIndexPosition;

        if (beatIndex >= beatMarkers.Count)
        {
            beatIndex = 0;
            currentBeat = beatMarkers[0];
        }
        else
        {
            currentBeat = beatMarkers[beatIndex];
        }

        HandleCurrentNote();
        HandleMissedNotes();
        HandleNewPhrase();
        HandleNewBar();


        float _analog;
        if (Conductor.instance.loopBeatIndexPositionAnalog < 1)
            _analog = Conductor.instance.loopBeatIndexPositionAnalog;

        else _analog = Conductor.instance.loopBeatIndexPositionAnalog % Conductor.instance.loopBeatIndexPosition;
        //Debug.Log("Analog : " + _analog);
        metronomeLine.anchoredPosition = Vector3.Lerp(currentBeat.anchoredPosition, nextBeat.anchoredPosition, _analog);

        oldBeatIndex = beatIndex;
        oldNote = currentNote;
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

    private void HandleCurrentNote()
    {
        //Select note of reference based on Mathf.Round - if target is near to the next note, use next note instead
        beatIndexSelector = (int)Mathf.Round(Conductor.instance.loopBeatIndexPositionAnalog);
        beatIndexSelector = Mathf.Clamp(beatIndexSelector, 0, beatMarkers.Count);

        //If upcoming note is at the next phrase, prepare for it.
        if (beatIndexSelector >= beatMarkers.Count)
        {
            beatIndexSelector = 0;
        }

        currentNote = beatMarkers[beatIndexSelector].GetComponent<IPlayerInteractable>();
        debugCurrentNote = beatMarkers[beatIndexSelector].gameObject;
    }

    public HitType CheckIfInputIsOnBeat()
    {
        float inputPressDistanceFromBeat = Mathf.Abs(beatIndexSelector - Conductor.instance.loopBeatIndexPositionAnalog);
        Debug.Log("Input: " + inputPressDistanceFromBeat);

        if (inputPressDistanceFromBeat < perfectHitThreshold)
        {
            return HitType.perfect;
        }
        else if (inputPressDistanceFromBeat < goodHitThreshold)
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
            }
        }
    }

    void HandleNewBar(){
        if (oldBeatIndex != beatIndex){
            barIndex++;

            if (barIndex >= 4){
                barIndex = 0;
                Events.BarEnded?.Invoke();
                Debug.Log("Bar End");
            }
        }
       
    }

    void HandleNewPhrase()
    {
        if (oldBeatIndex != beatIndex && beatIndex == 0)
        {
            Events.PhraseEnded?.Invoke();
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
