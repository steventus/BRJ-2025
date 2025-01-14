using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackFactory : MonoBehaviour
{
    public static TrackFactory instance = null;
    public float DistanceBetweenBeat = 50;
    public HorizontalLayoutGroup horizontalLayoutGroup;

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
        horizontalLayoutGroup.spacing = DistanceBetweenBeat;
    }

    void Update()
    {

    }

    void OnValidate()
    {
        horizontalLayoutGroup.spacing = DistanceBetweenBeat;
    }
}
