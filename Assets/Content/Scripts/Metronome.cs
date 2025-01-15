using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Metronome : MonoBehaviour
{
    [SerializeField] Conductor conductor;
    [SerializeField] Transform metronomeLine;
    [SerializeField] Transform[] beatMarkers;
    int beatIndex;
    public Transform currentBeat;
    public Transform nextBeat;
    public float lerpSpeed;

    public GameObject hitMessage;
    public GameObject missMessage;

    public bool isPlayerTurn = false;
    // SUCCESSFUL INPUT EVENT
    public UnityAction OnSuccessfulNoteHit;

    void Update()
    {
        beatIndex = Mathf.Clamp(beatIndex, 0, beatMarkers.Length);
        beatIndex = Mathf.FloorToInt(conductor.loopPositionInBeats);
        
        currentBeat = beatMarkers[beatIndex];
        if(beatIndex >= beatMarkers.Length) {
            nextBeat = beatMarkers[0];
        }
        else {
            nextBeat = beatMarkers[beatIndex];
        }

        metronomeLine.position = Vector3.Lerp(metronomeLine.position, nextBeat.position, Time.deltaTime * lerpSpeed);


        // [TEMP INPUT CONTROLS] //
        if(!isPlayerTurn) return;
        if(Input.GetKeyDown(KeyCode.Space)) {
            CheckIfInputIsOnBeat();
        }
    }

    void CheckIfInputIsOnBeat() {
        float minThresholdForNoteHit = 0.4f;
        float inputPressDistanceFromBeat = Mathf.Abs((float)beatIndex - conductor.loopPositionInBeats); 
        if(inputPressDistanceFromBeat < minThresholdForNoteHit) {
            hitMessage.SetActive(true);
            missMessage.SetActive(false);

            //invoke successful input event
            OnSuccessfulNoteHit?.Invoke();
        }
        else {
            hitMessage.SetActive(false);
            missMessage.SetActive(true);
         
            //invoke failed input event
        }

        Invoke("DisableMessages", 0.25f);
    }

    void DisableMessages() {
        hitMessage.SetActive(false);
        missMessage.SetActive(false);
    }
}
