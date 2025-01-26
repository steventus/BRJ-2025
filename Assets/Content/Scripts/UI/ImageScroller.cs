using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScroller : MonoBehaviour
{
    RawImage image;
    [SerializeField] Vector2 scrollDirection;
    void Start() {
        image = GetComponent<RawImage>();
    }
    
    // Update is called once per frame
    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + scrollDirection * Time.deltaTime, image.uvRect.size);
    }
}
