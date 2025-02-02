using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class TurntableManager : MonoBehaviour
{
    public static TurntableManager instance = null;

    //Disc Scratching Variables
    [Header("Disc")]
    GameObject[] discs;
    public LayerMask whatIsDisc;
    private bool isRotatingOnItsOwn = false; // Flag to indicate autonomous rotation
    private float currentAngle = 0f; // Track the current angle
    [HideInInspector] public float rotationSpeed = 0f; // Speed of autonomous rotation (degrees per second)
    private Vector3 previousMousePosition;
    [HideInInspector] public float rotationDirection = 0f;
    private float previousAngle = 0f; // Track the previous angle
    private Vector3 angularVelocity; // Track how fast the player was rotating the disc
    private float lastMouseInteractionTime = 0f; // Time of the last mouse movement
    private bool isBeingRotated;
    public float decelerationRate = 50f; // Deceleration rate (degrees per second^2)
    [Header("Scratch Input Thresholds")]
    public float scratchRotationThreshold;

    [Header("Metronome Input")]
    public Metronome metronome;

    //Disc Change Variables
    [Header("Disc Change")]
    public GameObject currentDisc;
    public Vector3 currentDiscPos, currentDiscScale;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        discs = GameObject.FindGameObjectsWithTag("Disc");

        for (int i = 0; i < discs.Length; i++)
        {
            if (discs[i].GetComponent<Disc>().isBeingPlayed)
            {
                currentDisc = discs[i];
                currentDiscPos = discs[i].transform.position;
                currentDiscScale = discs[i].transform.localScale;
                break;
            }
        }
    }
    private void Update()
    {
        Scratch();
    }

    public bool OnInputDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        return Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out raycastHit, Mathf.Infinity, whatIsDisc);
    }

    public bool OnInputUp()
    {
        //Let go of mouse button anywhere and it will still count as input up
        return Input.GetMouseButtonUp(0);
    }

    //TODO: Change this to bool scratch, create sensitivity variables to check for scratch sensitivity
    public void Scratch()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit, Mathf.Infinity, whatIsDisc);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 1000);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out raycastHit, Mathf.Infinity, whatIsDisc))
        {
            isBeingRotated = true;
        }

        if (isBeingRotated) // 0 is the left mouse button
        {

            Plane groundPlane = new Plane(Vector3.up, currentDisc.transform.position);

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 targetPoint = ray.GetPoint(enter);
                Vector3 direction = (targetPoint - currentDisc.transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

                // Apply the rotation to the disc
                float previousAngle = currentDisc.transform.rotation.eulerAngles.y;
                float newAngle = targetRotation.eulerAngles.y;
                float rotationDiff = newAngle - previousAngle;
                Debug.Log("rotationDiff: " + rotationDiff);

                currentDisc.transform.rotation = targetRotation;

                // Calculate angular velocity
                Vector3 deltaMouse = Input.mousePosition - previousMousePosition;
                angularVelocity = deltaMouse / Time.deltaTime;

                // Update the previous mouse position
                previousMousePosition = Input.mousePosition;

                rotationDirection = rotationDiff >= 0 ? -1f : 1f;

                // Disable autorotate while the mouse is pressed
                isRotatingOnItsOwn = false;
            }
        }
        if (!isRotatingOnItsOwn) // Transition to autonomous rotation after releasing the mouse
        {
            // Use the last angular velocity as the initial rotation speed
            rotationSpeed = angularVelocity.magnitude * rotationDirection;
        }

        // Rotate autonomously with deceleration
        if (isRotatingOnItsOwn)
        {
            // Apply a self-spin to the GameObject
            currentDisc.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Decelerate the rotation speed
            if (rotationSpeed > 0)
            {
                rotationSpeed -= decelerationRate * Time.deltaTime;
                rotationSpeed = Mathf.Max(rotationSpeed, 0); // Clamp to zero
            }
            if (rotationSpeed < 0)
            {
                rotationSpeed += decelerationRate * Time.deltaTime;
                rotationSpeed = Mathf.Min(rotationSpeed, 0);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isRotatingOnItsOwn = true;
            isBeingRotated = false;
        }

    }

    public ScratchDirection.Direction ScratchInput()
    {
        //By default, return false at all times.
        //Debug.Log("isBeingRotated: " + isBeingRotated + ". angularVelocity.magnitude: " +  angularVelocity.magnitude * Time.deltaTime);
        if (isBeingRotated && angularVelocity.magnitude * Time.deltaTime >= scratchRotationThreshold)
        {
            if (rotationDirection > 0) return ScratchDirection.Direction.ACW;

            else if (rotationDirection < 0) return ScratchDirection.Direction.CW;

            else return ScratchDirection.Direction.NoScratch;
        }
        return ScratchDirection.Direction.NoScratch;
    }
}
