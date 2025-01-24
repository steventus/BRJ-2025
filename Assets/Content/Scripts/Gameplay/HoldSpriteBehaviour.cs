using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldSpriteBehaviour : NoteSpriteBehaviour
{
    [SerializeField] private HoldNote holdNote;
    [SerializeField] protected Sprite holdEndSpriteCW, holdEndSpriteACW;

    void Start()
    {
        noteBehaviour = GetComponent<IScratchDirection>();
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (holdNote.IsStart)
        {
            image.sprite = noteBehaviour.GetScratchDirection() == ScratchDirection.Direction.CW ? spriteCW : spriteACW;
        }

        else
        {
            image.sprite = noteBehaviour.GetScratchDirection() == ScratchDirection.Direction.CW ? holdEndSpriteCW : holdEndSpriteACW;
        }
    }
}
