using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchNote : MonoBehaviour, IPlayerInteractable, IScratchDirection
{
    public bool isRight;
    public bool isHit;
    private ScratchDirection.Direction noteDirection => isRight ? ScratchDirection.Direction.CW : ScratchDirection.Direction.ACW;
    public void OnInputDown()
    {

    }

    public void OnScratch(ScratchDirection.Direction scratchDirection)
    {
        if (isHit)
            return;

        if (noteDirection != scratchDirection)
        {
            OnMiss();
            return;
        }

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

    public ScratchDirection.Direction GetScratchDirection(){
        return noteDirection;
    }

}
