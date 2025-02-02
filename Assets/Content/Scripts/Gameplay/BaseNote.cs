using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseNote : MonoBehaviour
{
    public bool isHit;
    public UnityEvent noteHitEvent;

    public void UpdateNoteHit()
    {
        isHit = true;
        noteHitEvent?.Invoke();
    }
}
