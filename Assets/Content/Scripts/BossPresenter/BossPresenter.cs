using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossPresenter : MonoBehaviour
{
    public Sprite scratchNoteSprite;
    public Sprite holdNoteSprite;
    public Sprite badNoteSprite;
    public Sprite changeNoteSprite;
    public enum DancePose
    {
        scratchNote,
        holdNote,
        badNote,
        changeNote,
    }
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {

    }

    void Update()
    {

    }
    //TODO: Pull from judgement line/notes hit with observer design pattern
    public void PerformDance(DancePose dancePose, bool ifRight = false)
    {
        spriteRenderer.flipY = ifRight;

        //Choose corresponding sprite based on dancePose received
        //Todo: Need to be connected with Kato's script closer
        switch (dancePose)
        {
            case DancePose.scratchNote:
                spriteRenderer.sprite = scratchNoteSprite;
                break;

            case DancePose.holdNote:
                spriteRenderer.sprite = holdNoteSprite;
                break;

            case DancePose.badNote:
                spriteRenderer.sprite = badNoteSprite;
                break;

            case DancePose.changeNote:
                spriteRenderer.sprite = changeNoteSprite;
                break;
        }
    }
}
