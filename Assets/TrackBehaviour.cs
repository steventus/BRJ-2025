using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBehaviour : MonoBehaviour
{
    public float MinLength; //Base length at minimum notes
    public float LengthPerBeat; //Additional length (gradient) per additional notes
    int childCount;
    void Start()
    {
        childCount = transform.childCount;
    }

    void Update()
    {
        
    }

    void AdjustLength(){

    }

    void OnValidate(){
        childCount = transform.childCount;
    }
}
