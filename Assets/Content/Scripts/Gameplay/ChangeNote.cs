using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeNote : MonoBehaviour, IPlayerInteractable
{

    public void OnInputDown()
    {
        Debug.Log("Change!");
    }

    public void OnInputUp()
    {

    }

    public void OnMiss()
    {
        Debug.Log("Bad!");
        Debug.Log("Change!");
    }
}
