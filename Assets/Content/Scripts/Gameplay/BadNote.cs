using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadNote : MonoBehaviour, IPlayerInteractable
{
    public void OnInputDown()
    {
        Debug.Log("Bad!");
    }

    public void OnInputUp()
    {

    }

    public void OnMiss()
    {
    }
}
