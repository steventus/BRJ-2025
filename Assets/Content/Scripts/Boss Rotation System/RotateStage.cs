using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotateStage : MonoBehaviour
{
    public bool shouldRotate = false;
    public bool rotateClockwise = true;
    public float yRot = 5;
    
    void Update()
    {
        if(!rotateClockwise) {
            yRot *= -1;
        }

        if(shouldRotate)
            transform.Rotate(0, yRot * Time.deltaTime, 0);
    }

    public void EnableRotation(bool stop) {
        shouldRotate = stop;
    }
}
