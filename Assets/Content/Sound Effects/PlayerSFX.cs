using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSFX : MonoBehaviour
{
    public UnityEvent PlayScratchSFX;
    public UnityEvent PlayHurtSFX;
    public UnityEvent PlayClickSFX;
    void OnEnable()
    {
        Events.OnSuccessfulNoteHit += ScratchSFX;

        Events.OnBadNoteHit += HurtSFX;
        Events.OnUnsuccessfulNoteHit += HurtSFX;
    }
    void OnDisable()
    {
        Events.OnSuccessfulNoteHit -= ScratchSFX;
    
        Events.OnBadNoteHit -= HurtSFX;
        Events.OnUnsuccessfulNoteHit -= HurtSFX;
    }

    void Update()
    {
        if(TurntableManager.instance.OnInputDown())
        {
            PlayClickSFX?.Invoke();
        }
    }

    void ScratchSFX(int n)
    {
        PlayScratchSFX?.Invoke();
    }

    void HurtSFX(int n) 
    {
        PlayHurtSFX?.Invoke();
    }
}
