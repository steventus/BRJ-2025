using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class TurntableManager : MonoBehaviour
{
    public Camera cam;
    public float raycastRange = 100f;
    //Disc Scratching Variables
    [Header("Disc")]
    public GameObject disc;
    RotateStage discRotater;
    public LayerMask whatIsDisc;
    private bool isRotatingOnItsOwn = false; // Flag to indicate autonomous rotation
    private float currentAngle = 0f; // Track the current angle
    private float rotationSpeed = 0f; // Speed of autonomous rotation (degrees per second)

    private float previousAngle = 0f; // Track the previous angle
    private float angularVelocity = 0f; // Track how fast the player was rotating the disc
    public float decelerationRate = 50f; // Deceleration rate (degrees per second^2)
    private float lastMouseInteractionTime = 0f; // Time of the last mouse movement

    bool isScratching = false;
    void Awake() {
        discRotater = disc.GetComponent<RotateStage>();
    }
    void Start() {
        cam = Camera.main;
    }

    void Update() {
        if(isScratching) {
            discRotater.rotateClockwise = false;
            discRotater.EnableRotation(false);
        }
        else {
            discRotater.rotateClockwise = true;
            discRotater.EnableRotation(true);
        }
    }

    public bool Scratch()
    {
        isScratching = false;

        // raycast check for disc
        Vector3 mouseScreenPos = Input.mousePosition;

        Ray ray = cam.ScreenPointToRay(mouseScreenPos);
        RaycastHit hit;
        
        bool hitDisc = Physics.Raycast(ray, out hit, raycastRange, whatIsDisc);

        if(Input.GetMouseButtonDown(0) && hitDisc) {
            isScratching = true;

            // Calculate the direction from the GameObject to the mouse
            Vector3 direction = hit.point - hit.transform.position;

            // Ignore the Z axis for 2D rotation
            direction.z = 0;

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // Apply the rotation to the GameObject
            hit.transform.localRotation = Quaternion.Euler(0, angle, 0);
        }

        return isScratching; 
    }
}
