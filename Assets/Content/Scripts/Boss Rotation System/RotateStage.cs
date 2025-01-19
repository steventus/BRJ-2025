using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotateStage : MonoBehaviour
{
    public bool shouldRotate = false;
    public float yRot = 5;
    
    void Update()
    {
        if(shouldRotate)
            transform.Rotate(0, yRot * Time.deltaTime, 0);
    }
}
