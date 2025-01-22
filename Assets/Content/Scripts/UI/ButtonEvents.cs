using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI tmpComponent;
    public Color selectedColor, unselectedColor;
    public float scaler;
    public float zRotation;

    void Awake() {
        tmpComponent = GetComponentInChildren<TextMeshProUGUI>();

        if(tmpComponent != null)
            tmpComponent.color = unselectedColor;
    }

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(tmpComponent != null)
            tmpComponent.color = selectedColor;

        transform.localScale = transform.localScale * scaler;
        transform.rotation = Quaternion.Euler(new Vector3(0,0,zRotation));

    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if(tmpComponent != null)
            tmpComponent.color = unselectedColor;
    
        transform.localScale = transform.localScale / scaler;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
