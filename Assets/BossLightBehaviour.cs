using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLightBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject lightObj;

    public void SetLight(bool _state){
        lightObj.SetActive(_state);
    }   
    
}
