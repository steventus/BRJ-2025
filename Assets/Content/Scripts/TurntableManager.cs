using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private float previousAngle = 0f; // Track the previous angle
    private float angularVelocity = 0f; // Track how fast the player was rotating the disc
    private float lastMouseInteractionTime = 0f; // Time of the last mouse movement

    public TMP_Text e;


    public float decelerationRate = 50f; // Deceleration rate (degrees per second^2)



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

        for(int i = 0; i < discs.Length; i++)
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

        if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x != Mathf.Infinity)
        Scratch();

        VolumeHandling();
        TempoHandling();
    }

    void Scratch()
    {
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(MousePos.x, MousePos.y);
        RaycastHit2D hit;

        hit = Physics2D.Raycast(mousePos2d, Vector2.zero, Mathf.Infinity, whatIsDisc);

        if (Input.GetMouseButton(0) && Physics2D.Raycast(mousePos2d, Vector2.zero, Mathf.Infinity, whatIsDisc) && hit.transform.gameObject.GetComponent<Disc>().isBeingPlayed) 
        {


            // Get the mouse position in world space
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Calculate the direction from the GameObject to the mouse
            Vector3 direction = mousePosition - hit.transform.position;

            // Ignore the Z axis for 2D rotation
            direction.z = 0;

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation to the GameObject
            hit.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Calculate angular velocity (degrees per second)
            float deltaAngle = Mathf.DeltaAngle(previousAngle, angle);
            float deltaTime = Time.time - lastMouseInteractionTime;

            if (deltaTime > 0)
            {
                angularVelocity = deltaAngle / deltaTime; // Calculate angular velocity
            }

            // Store the angle and time for the next frame
            previousAngle = angle;
            lastMouseInteractionTime = Time.time;

            // Update the current angle
            currentAngle = angle;

            // Disable autonomous rotation while the mouse is pressed
            isRotatingOnItsOwn = false;
        }
        if (!isRotatingOnItsOwn) // Transition to autonomous rotation after releasing the mouse
        {
            // Use the last angular velocity as the initial rotation speed
            rotationSpeed = angularVelocity;
            isRotatingOnItsOwn = true;
        }
        if (isRotatingOnItsOwn)
        {
            // Increment the current angle using the rotation speed
            currentAngle += rotationSpeed * Time.deltaTime;

            // Apply the updated angle to the GameObject
            currentDisc.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

            // Decelerate the rotation speed
            if (rotationSpeed > 0)
            {
                rotationSpeed -= decelerationRate * Time.deltaTime;
                rotationSpeed = Mathf.Max(rotationSpeed, 0); // Clamp to zero
                e.text = "Is rotating Right";
            }
            else if (rotationSpeed < 0)
            {
                rotationSpeed += decelerationRate * Time.deltaTime;
                rotationSpeed = Mathf.Min(rotationSpeed, 0); // Clamp to zero
                e.text = "Is rotating Left";
            }
        }

        if (Input.GetMouseButtonDown(0) && Physics2D.Raycast(mousePos2d, Vector2.zero, Mathf.Infinity, whatIsDisc) && !hit.transform.gameObject.GetComponent<Disc>().isBeingPlayed)
        {
            changeDisc(hit);
        }


    }

    void VolumeHandling()
    {
        if(volumeSlider != null)
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
