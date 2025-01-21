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
                Metronome.instance.PerfectHit();
                break;

            case Metronome.HitType.good:
                Debug.Log("Correct!");
                isHit = true;
                Metronome.instance.GoodHit();
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
            Metronome.instance.MissHit();
            Debug.Log("Bad!");
        }
    }

}
