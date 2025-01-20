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
    //Disc Scratching Variables
    [Header("Disc")]
    GameObject[] discs;
    public LayerMask whatIsDisc;
    private bool isRotatingOnItsOwn = false; // Flag to indicate autonomous rotation
    private float currentAngle = 0f; // Track the current angle
    private float rotationSpeed = 0f; // Speed of autonomous rotation (degrees per second)
    private Vector3 previousMousePosition;
    private float rotationDirection = 0f;
    private float previousAngle = 0f; // Track the previous angle
    private Vector3 angularVelocity; // Track how fast the player was rotating the disc
    private float lastMouseInteractionTime = 0f; // Time of the last mouse movement
    private bool isBeingRotated;
    public float decelerationRate = 50f; // Deceleration rate (degrees per second^2)

    [Header("Metronome Input")]
    public Metronome metronome;

    //Volume Variables
    [Header("Volume")]
    public float maxVolume;
    public float inGameVolume;
    public Slider volumeSlider;

    //Tempo Variables
    [Header("Tempo")]
    public float maxTempo;
    public float inGameTempo;
    public Slider tempoSlider;

    //Disc Change Variables
    [Header("Disc Change")]
    public GameObject currentDisc;
    public Vector3 currentDiscPos, currentDiscScale;

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

        VolumeHandling();
        TempoHandling();
    }

    void Scratch()
    {
        //Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 mousePos2d = new Vector2(MousePos.x, MousePos.y);
        //RaycastHit2D hit;

        //hit = Physics2D.Raycast(MousePos, Camera.main.transform.forward, Mathf.Infinity, whatIsDisc);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit, Mathf.Infinity, whatIsDisc);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 1000);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out raycastHit, Mathf.Infinity, whatIsDisc)) isBeingRotated = true;

        if (isBeingRotated) // 0 is the left mouse button
        {
            
            Plane groundPlane = new Plane(Vector3.up, currentDisc.transform.position);

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 targetPoint = ray.GetPoint(enter);
                Vector3 direction = (targetPoint - currentDisc.transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

                // Apply the rotation to the disc
                currentDisc.transform.rotation = targetRotation;

                // Calculate angular velocity
                Vector3 deltaMouse = Input.mousePosition - previousMousePosition;
                angularVelocity = deltaMouse / Time.deltaTime;

                // Update the previous mouse position
                previousMousePosition = Input.mousePosition;

                rotationDirection = deltaMouse.x >= 0 ? -1f : 1f;

                // Disable autorotate while the mouse is pressed
                isRotatingOnItsOwn = false;
            }        
        }
        if (!isRotatingOnItsOwn) // Transition to autonomous rotation after releasing the mouse
        {
            // Use the last angular velocity as the initial rotation speed
            rotationSpeed = angularVelocity.magnitude * rotationDirection;
            isRotatingOnItsOwn = true;
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

        if (Input.GetMouseButtonUp(0)) isBeingRotated = false;
    }

    void VolumeHandling()
    {
        if (volumeSlider != null)
        {
            volumeSlider.maxValue = maxVolume;
            inGameVolume = volumeSlider.value;
            volumeSlider.minValue = 0f;
        }
    }

    void TempoHandling()
    {
        if (tempoSlider != null)
        {
            tempoSlider.maxValue = maxTempo;
            inGameTempo = tempoSlider.value;
            tempoSlider.minValue = 0f;
        }
    }

    void changeDisc(RaycastHit2D hit)
    {
        currentDisc.transform.position = hit.transform.position;
        currentDisc.transform.localScale = hit.transform.localScale;
        currentDisc.transform.gameObject.GetComponent<Disc>().isBeingPlayed = false;
        hit.transform.position = currentDiscPos;
        hit.transform.localScale = currentDiscScale;
        currentDisc = hit.transform.gameObject;
        hit.transform.gameObject.GetComponent<Disc>().isBeingPlayed = true;

    }
}
