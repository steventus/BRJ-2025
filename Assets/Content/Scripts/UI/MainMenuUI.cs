using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public Transform[] scenePositions;
    Vector3 targetPosition;
    public float speed;
    public void MoveCamera(int index) {
        targetPosition = scenePositions[index].position;
    }
    void Start() {
        targetPosition = scenePositions[0].position;
    }
    void Update() {
        Vector3 lerp = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        lerp.z = -10;
        transform.position = lerp;
    }
}
