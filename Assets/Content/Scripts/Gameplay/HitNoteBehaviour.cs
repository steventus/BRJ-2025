using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HitNoteBehaviour : MonoBehaviour
{
    public BaseNote note;

    void Start()
    {
        note = GetComponent<BaseNote>();
    }
    void OnEnable()
    {
        note.noteHitEvent?.AddListener(OnNoteHit);
    }

    void OnDisable()
    {
        note.noteHitEvent?.RemoveListener(OnNoteHit);
    }

    private void OnNoteHit()
    {
        Image _image = GetComponentInChildren<Image>();
        Color _Color = _image.color;
        _Color.a = 0.2f;
        _image.color = _Color;
    }
}
