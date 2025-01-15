using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementLineBehaviour : MonoBehaviour
{

    public BoxCollider2D thisCollider;
    [SerializeField] private float perfectDistance = 50;
    [SerializeField] private float goodDistance = 70;
    [SerializeField] private float missDistance = 100;


    private Conductor conductor = Conductor.instance;
    private TrackFactory trackFactory = TrackFactory.instance;
    private float speed = 0;
    private RectTransform thisRectTransform;
    private bool testingIfMoving = false;

    void Awake()
    {
        thisRectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {

    }

    void Update()
    {
        thisRectTransform.anchoredPosition += Vector2.right * speed * Time.deltaTime;

        #region Test - TO BE REMOVED IN PRODUCTION
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (testingIfMoving)
            {
                testingIfMoving = false;
                GetComponent<SyncController>().QueueForBeat(StopMoving);
            }

            else
            {
                testingIfMoving = true;
                GetComponent<SyncController>().QueueForBeat(StartMoving);
            }
        }
        #endregion
    }

    public void StartMoving()
    {
        // Calculate ideal speed to move
        // speed = (Physical distance between each "Green circle/UI Beat" set by Trackfactory) multiply by (Beats per second set by Conductor).
        speed = (trackFactory.DistanceBetweenBeat + trackFactory.DistanceOffset) * conductor.songBpm / 60;
        Debug.Log(speed);

        //Any zero-error based on current time to starting beat

    }

    public void StopMoving()
    {
        speed = 0;
    }

    public void ResetPosition()
    {
        //Reset to first position based on Trackfactory
    }

    /// <summary>
    /// This is called when the player does an input.
    /// </summary>
    public void JudgeNote()
    {
        //Judge perfect/good/miss based on what note a player hits.

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(perfectDistance, 10,10));

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(goodDistance, 8,8));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(missDistance, 6,6));
    }

}
