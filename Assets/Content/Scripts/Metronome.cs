using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    [SerializeField] RectTransform metronomeLine;
    [SerializeField] RectTransform[] beatMarkers;
    int beatIndex;
    [SerializeField] Conductor conductor;

    void Update()
    {
        beatIndex = Mathf.Clamp(beatIndex, 0, beatMarkers.Length);
        metronomeLine.anchoredPosition = beatMarkers[beatIndex].anchoredPosition;

        beatIndex = Mathf.FloorToInt(conductor.loopPositionInBeats);
    }

}
