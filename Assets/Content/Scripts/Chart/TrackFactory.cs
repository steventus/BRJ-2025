using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
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

    // [[ JOHNNY - adding charts to spawn ]]
// ================================================================ //

    public Chart chartToSpawn;

// ================================================================ //

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
        // [[ JOHNNY - adding charts to spawn ]]
// ================================================================ //
        for (int i = 0; i <= chartToSpawn.notes.Count - 1; i++)
        {
            Note _currentSprite = chartToSpawn.notes[i];

// ================================================================ //

            GameObject _obj = Instantiate(notePrefab);

            Image _objImage = _obj.GetComponent<Image>();
            _objImage.sprite = _currentSprite.GetComponentInChildren<SpriteRenderer>().sprite;
            _objImage.color = _currentSprite.GetComponentInChildren<SpriteRenderer>().color;

            _obj.transform.SetParent(track, false);

            notes.Add(_obj);
        }

        AdjustTrackLength();
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
