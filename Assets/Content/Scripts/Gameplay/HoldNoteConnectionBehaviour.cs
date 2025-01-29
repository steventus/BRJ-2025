using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldNoteConnectionBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject holdNoteConnectionPrefab;
    [SerializeField] private Color CWColour, ACWColor;
    private HoldNote holdNote;
    private GameObject holdNoteConnection = null;
    void Start()
    {
        holdNote = GetComponent<HoldNote>();

        if (holdNote.ConnectedEndHoldNote != null)
            CreateConnection(holdNote.ConnectedEndHoldNote);
    }

    public void CreateConnection(HoldNote endingHoldNote)
    {
        if (!holdNote.IsStart)
            return;

        //Instantiate something with dimensions equal to this connection
        holdNoteConnection = Instantiate(holdNoteConnectionPrefab, holdNote.transform);
        RectTransform _transform = holdNoteConnection.GetComponent<RectTransform>();

        //Set position
        _transform.anchoredPosition = new Vector2(-holdNote.GetComponent<RectTransform>().rect.x/2, _transform.anchoredPosition.y);

        //Set length
        SetLength(endingHoldNote);

        //Set to first in local hierarchy so that it appears behind the note sprites (Unity UI rendering stacking)
        holdNoteConnection.transform.SetAsFirstSibling();

        //Set colour based on CW/ACW
        holdNoteConnection.GetComponent<Image>().color = holdNote.isCW ? CWColour : ACWColor;
    }

    private void SetLength(HoldNote endingHoldNote)
    {
        StartCoroutine(CalculateDistance(endingHoldNote));
    }

    //Canvas doesn't update fast enough
    IEnumerator CalculateDistance(HoldNote endingHoldNote)
    {
        Canvas.ForceUpdateCanvases();
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        float _length = Vector3.Distance(endingHoldNote.GetComponent<RectTransform>().position, holdNote.GetComponent<RectTransform>().position) + endingHoldNote.GetComponent<RectTransform>().rect.x;
        
        holdNoteConnection.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _length);
    }
}
