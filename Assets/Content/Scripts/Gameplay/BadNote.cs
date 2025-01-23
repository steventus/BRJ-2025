using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadNote : MonoBehaviour, IPlayerInteractable
{
    public bool isHit;
    public void OnInputDown()
    {

    }

    public void OnScratch(ScratchDirection.Direction scratchDirection)
    {
        if (isHit)
            return;

        Debug.Log("Hitting Bad Note");

        switch (Metronome.instance.CheckIfInputIsOnBeat())
        {
            case Metronome.HitType.perfect:
                isHit = true;
                Metronome.instance.BadHit();
                break;

            case Metronome.HitType.good:
                isHit = true;
                Metronome.instance.BadHit();
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
        }
    }
}
