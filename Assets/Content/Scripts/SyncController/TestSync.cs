using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSync : MonoBehaviour
{
    private SyncController syncController;
    public int QueueBeatDelay = 1;
    void Start()
    {
        syncController = GetComponent<SyncController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightBracket)){
            syncController.QueueForBeat(MoveRight,QueueBeatDelay);
        }   

        if (Input.GetKeyDown(KeyCode.LeftBracket)){
            syncController.QueueForBeat(MoveLeft,QueueBeatDelay);
        }   
    }

    private void MoveRight(){
        transform.position = transform.position + Vector3.right;
        Debug.Log("test");
    }

    private void MoveLeft(){
        transform.position = transform.position + Vector3.left;
        Debug.Log("test");
    }
}
