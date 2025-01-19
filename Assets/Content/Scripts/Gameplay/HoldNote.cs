using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNote : MonoBehaviour, IPlayerInteractable
{
    public bool isRight;
    private bool isStart = true;

    //Called and set from TrackFactory
    public void SetStartHold()
    {
        //If it is already an start note, skip everything
        if (isStart)
        {
            Debug.Log("Error, current hold note is already a start note");
            return;
        }

        isStart = true;
    }
    //Called and set from TrackFactory
    public void SetEndHold()
    {
        //If it is already an end note, skip everything
        if (!isStart)
        {
            Debug.Log("Error, current hold note is already an end note");
            return;
        }

        isStart = false;
    }


    public void OnInputDown()
    {
        if (isStart)
        {
            Debug.Log("Holding!");
        }
    }

    public void OnInputUp()
    {
        if (isStart)
        {
            Debug.Log("Bad!");
        }

        else
        {
            Debug.Log("Good!");
        }
    }
    public void OnMiss()
    {
        Debug.Log("Bad!");
    }
}
