using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TrackFactory : MonoBehaviour
{
    public static TrackFactory instance = null;
    public GameObject notePrefab;
    public RectTransform track;
    public GameObject currentChartPrefab;
    private Chart currentChart => currentChartPrefab.GetComponent<Chart>();
    public List<GameObject> notes;

    [Header("Dynamic Track Generation Settings")]
    //Set Track Length
    public float MinLength; //Base length at minimum notes
    public float LengthPerBeat; //Additional length (gradient) per additional notes
    public float trackHeight;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
    }

    void Update()
    {
        #region TODO: REMOVE WHEN FINISHED
        if (Input.GetKeyDown(KeyCode.U))
        {
            CreateTrack();
        }
        #endregion
    }

    public void CreateTrack()
    {
        ClearTrack();

        //Receive data from ChartMaker and instantiate new notes under track
        for (int i = 0; i <= currentChart.notes.Count - 1; i++)
        {
            Note _currentNote = currentChart.notes[i];

            if (_currentNote.noteType == NoteType.Note.empty)
                continue;

            GameObject _obj = Instantiate(notePrefab);

            //GameObject _obj = Instantiate(currentChart.notes[i].gameObject,track);

            Image _objImage = _obj.GetComponent<Image>();
            _objImage.sprite = _currentNote.GetComponentInChildren<SpriteRenderer>().sprite;
            _objImage.color = _currentNote.GetComponentInChildren<SpriteRenderer>().color;

            _obj.transform.SetParent(track, false);


            notes.Add(_obj);
        }

        ApplyHoldNotes();

        AdjustTrackLength();
    }

    public void ApplyHoldNotes()
    {
        for (int i = 0; i <= notes.Count; i++)
        {
            Note _currentNote = notes[i].GetComponent<Note>();

            //Check if current selected note is connected. 
            if (_currentNote.isConnected)
            {
                //If it is, additionally check if its already assigned.
                if (_currentNote.noteType != NoteType.Note.holdStart || _currentNote.noteType != NoteType.Note.holdEnd)
                    continue;

                Note _connectedNote = notes[_currentNote.connectedNoteIndex].GetComponent<Note>();

                LineRenderer lr = _currentNote.gameObject.AddComponent<LineRenderer>();
                lr.SetPosition(0, _currentNote.transform.position);
                lr.SetPosition(1, _connectedNote.transform.position);
                _currentNote.SetNoteType(NoteType.Note.holdStart);
                _connectedNote.SetNoteType(NoteType.Note.holdEnd);
            }
        }
    }

    void AdjustTrackLength()
    {
        //Adjust distance
        GetComponent<RectTransform>().sizeDelta = new Vector2(MinLength + notes.Count * LengthPerBeat, trackHeight);
    }

    void ClearTrack()
    {
        for (int i = notes.Count - 1; i >= 0; i--)
        {
            Destroy(notes[i].gameObject);
        }

        notes.Clear();
    }

    void OnValidate()
    {
        AdjustTrackLength();
    }
}
