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
    [SerializeField] GameObject[] discs;
    public LayerMask whatIsDisc;
    private bool isRotatingOnItsOwn = true; // Flag to indicate autonomous rotation
    private float currentAngle = 0f; // Track the current angle
    private float rotationSpeed = 0f; // Speed of autonomous rotation (degrees per second)

    private float previousAngle = 0f; // Track the previous angle
    private float angularVelocity = 0f; // Track how fast the player was rotating the disc
    private float lastMouseInteractionTime = 0f; // Time of the last mouse movement

    [SerializeField]
    private float decelerationRate = 50f; // Deceleration rate (degrees per second^2)



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

    private void Update()
    {
        if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x != Mathf.Infinity)
        Scratch();
    }

    void Scratch()
    {
        GameObject go;
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(MousePos.x, MousePos.y);
        RaycastHit2D hit;

        hit = Physics2D.Raycast(mousePos2d, Vector2.zero, Mathf.Infinity, whatIsDisc);


        if (Input.GetMouseButton(0) && Physics2D.Raycast(mousePos2d, Vector2.zero, Mathf.Infinity, whatIsDisc)) 
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
            if(rotationSpeed == 0) {
                rotationSpeed = -250;
            }
            // Increment the current angle using the rotation speed
            currentAngle += rotationSpeed * Time.deltaTime;

            // Apply the updated angle to the GameObject
            foreach(GameObject disc in discs) {
                disc.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }

            // Decelerate the rotation speed
            if (rotationSpeed > 0)
            {
                rotationSpeed -= decelerationRate * Time.deltaTime;
                rotationSpeed = Mathf.Max(rotationSpeed, 0); // Clamp to zero
            }
            else if (rotationSpeed < 0)
            {
                rotationSpeed += decelerationRate * Time.deltaTime;
                rotationSpeed = Mathf.Min(rotationSpeed, 0); // Clamp to zero
            }
        }
    }

}
