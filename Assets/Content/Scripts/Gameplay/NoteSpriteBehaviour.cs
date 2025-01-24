using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteSpriteBehaviour : MonoBehaviour
{
    [SerializeField] protected Image image;
    [SerializeField] protected IScratchDirection noteBehaviour;
    [SerializeField] protected Sprite spriteCW, spriteACW;
    void Start()
    {
        noteBehaviour = GetComponent<IScratchDirection>();
        UpdateSprite();
    }

    void UpdateSprite(){
        image.sprite = noteBehaviour.GetScratchDirection() == ScratchDirection.Direction.CW ? spriteCW : spriteACW;
    }
}
