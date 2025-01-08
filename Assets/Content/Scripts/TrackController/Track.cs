using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;

    void OnValidate(){
        startPos = transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPos, endPos);
    }
}

