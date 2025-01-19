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
                StopMoving();
                thisRectTransform.anchoredPosition = trackFactory.notes[0].GetComponent<RectTransform>().anchoredPosition;
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
        //Distance between 2 beats
        float _distance = Vector2.Distance(TrackFactory.instance.notes[0].GetComponent<RectTransform>().anchoredPosition, TrackFactory.instance.notes[1].GetComponent<RectTransform>().anchoredPosition);

        // Calculate ideal speed to move
        // speed = (Physical distance between each "Green circle/UI Beat" set by Trackfactory) multiply by (Beats per second set by Conductor).
        speed = (_distance) * conductor.songBpm / 60;
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
        List<Collider2D> results = new List<Collider2D>();
        thisCollider.OverlapCollider(new ContactFilter2D().NoFilter(), results);



    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(perfectDistance, 10, 10));

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(goodDistance, 8, 8));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(missDistance, 6, 6));
    }

}
