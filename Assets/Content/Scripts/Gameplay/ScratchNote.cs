using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchNote : MonoBehaviour, IPlayerInteractable
{
    public bool isRight;
    public bool isHit;

    public void OnInputDown()
    {
        if (isHit)
            return;
            
        switch (Metronome.instance.CheckIfInputIsOnBeat())
        {
            case Metronome.HitType.perfect:
                Debug.Log("Perfect!");
                isHit = true;
                break;

            case Metronome.HitType.good:
                Debug.Log("Correct!");
                isHit = true;
                break;

            case Metronome.HitType.miss:
                OnMiss();
                break;
        }
    }

    public void OnInputUp()
    {

    }

    public void OnMiss()
    {
        if (!isHit)
        {
            isHit = true;
            Debug.Log("Bad!");
        }
    }

}
