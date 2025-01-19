using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNote : MonoBehaviour, IPlayerInteractable
{
    public bool isRight;
    private bool isStart = false;
    void Start()
    {
    }

    void FindEndNote()
    {
        //If it is already an end note, skip everything
        if (!isStart)
            return;

        isStart = true;

        //TODO:In the chart, find the next hold Note
    }


    public void OnInputDown()
    {
        if (isStart)
        {
            Debug.Log("Good!");
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
