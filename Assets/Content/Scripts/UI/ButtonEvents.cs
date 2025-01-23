using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class ButtonEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI tmpComponent;
    //Assign image in inspector only
    public Image image;
    public Color selectedColor, unselectedColor;
    public float scaler;
    public float zRotation;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    void Awake() {
        tmpComponent = GetComponentInChildren<TextMeshProUGUI>();

        if(tmpComponent != null)
            tmpComponent.color = unselectedColor;

        if(image != null)
            image.color = unselectedColor;
    }

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(tmpComponent != null)
            tmpComponent.color = selectedColor;

        if(image != null)
            image.color = selectedColor;
            
        transform.localScale = transform.localScale * scaler;
        transform.rotation = Quaternion.Euler(new Vector3(0,0,zRotation));

        OnEnter?.Invoke();
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if(tmpComponent != null)
            tmpComponent.color = unselectedColor;

        if(image != null)
            image.color = unselectedColor;
            
        transform.localScale = transform.localScale / scaler;
        transform.rotation = Quaternion.Euler(Vector3.zero);
 
        OnExit?.Invoke();
    }
}
