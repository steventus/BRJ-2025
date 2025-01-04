using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPlatform : MonoBehaviour
{
    [SerializeField]
    float rotation = 0;
    [SerializeField]
    float rotationSpeed = 0;
    [SerializeField]
    bool rotateClockwise = false;
    void Start()
    {
        if(rotateClockwise) {
            rotationSpeed = -rotationSpeed;
        }

        StartCoroutine(RotateBody());
    }

    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, rotation);
    }

    IEnumerator RotateBody()
    {
        while(rotation < 360) {
            rotation += Time.deltaTime * rotationSpeed;
            yield return null;
        }    
        rotation = 0;
        StartCoroutine(RotateBody());   
    }
}
