using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchNote : MonoBehaviour, IPlayerInteractable
{
    public bool isRight;

    public void OnInputDown()
    {
        Debug.Log("Correct!");
    }

    public void OnInputUp()
    {

    }

    public void OnMiss(){
        Debug.Log("Bad!");
    }

}
