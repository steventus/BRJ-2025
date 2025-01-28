using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Transform playerHand;
    [SerializeField] Vector3 moveVector;
    RaycastHit hit;
    void OnEnable() 
    {
    }
    void OnDisable()
    {
    }
    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool testRay = Physics.Raycast(ray, out hit, Mathf.Infinity, TurntableManager.instance.whatIsDisc);
        
        if(testRay) 
        {
            Vector3 point = hit.point;
            if(TurntableManager.instance.OnInputDown()) 
            {
                point += moveVector;
            }
            playerHand.position = point;
        }
    }
}
