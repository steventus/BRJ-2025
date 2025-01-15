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

    public GameObject perfectMessage;
    public GameObject hitMessage;
    public GameObject missMessage;

    public float minThresholdForNoteHit = 0.4f;
    public float perfectHitThreshold = 0.2f;
    void Start() {
        beatIndex = 0;
    }
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
    }

    public void CheckIfInputIsOnBeat() {
        int perfectHitDmg = 3;
        int hitDmg = 1;

        float inputPressDistanceFromBeat = Mathf.Abs((float)beatIndex - conductor.loopPositionInBeats); 
        Debug.Log(inputPressDistanceFromBeat);
        if(inputPressDistanceFromBeat < perfectHitThreshold) {
            perfectMessage.SetActive(true);
            hitMessage.SetActive(false);
            missMessage.SetActive(false);
    
            Events.OnSuccessfulNoteHit?.Invoke(perfectHitDmg);
        }
        else if(inputPressDistanceFromBeat < minThresholdForNoteHit) {
            hitMessage.SetActive(true);
            perfectMessage.SetActive(false);
            missMessage.SetActive(false);

            //invoke successful input event
            Events.OnSuccessfulNoteHit?.Invoke(hitDmg);
        }
        else {
            hitMessage.SetActive(false);
            missMessage.SetActive(true);
         
            //invoke failed input event
            Events.OnUnsuccessfulNoteHit?.Invoke(hitDmg);
        }

        Invoke("DisableMessages", 0.25f);
    }

    void DisableMessages() {
        perfectMessage.SetActive(false);
        hitMessage.SetActive(false);
        missMessage.SetActive(false);
    }
}
