using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossRotationController : MonoBehaviour
{
    public List<BossBehaviour> bossList;
    private Queue<BossBehaviour> activeBossList = new Queue<BossBehaviour>();
    private BossBehaviour currentBoss;

    public Transform bossWheelPositionsParent;
    [SerializeField] List<Transform> bossWheelPositions;
    [SerializeField] bool swapTriggered = false;
    void Start()
    {
        //get all boss positions and store them in an array
        foreach(Transform t in bossWheelPositionsParent) {
            bossWheelPositions.Add(t);
        }

        for (int i = 0; i < bossList.Count; i++)
        {
            activeBossList.Enqueue(bossList[i]);

            //take all bosses in queue and place them at their respective positions
            bossList[i].transform.position = bossWheelPositions[i].position;
        }

        currentBoss = activeBossList.Dequeue();
    }

    void Update()
    {
        //TEST --- TO BE REMOVED AFTERWARDS
        #region TEST
        if (swapTriggered)
        {
            RotateNextBoss(currentBoss);
            swapTriggered = false;
        }
        #endregion

    }

    public BossBehaviour RotateNextBoss(BossBehaviour _currentBoss)
    {
        activeBossList.Enqueue(_currentBoss);

        BossBehaviour _nextBoss = activeBossList.Dequeue();
        //Guard for any bosses that are dead to be removed
        while (_nextBoss.isDead)
        {
            _nextBoss = activeBossList.Dequeue();
        }

        RepositionBosses();

        currentBoss = _nextBoss;
        return _nextBoss;
    }

    //rotate bosses so that next active boss is standing in front of player
    void RepositionBosses() {
        Debug.Log("swap bosses");

        //loop through the boss list & place bosses at the next position
        for(int i = 0; i < bossList.Count; i++)
        {
            Vector3 targetPosition = Vector3.zero;
            if(i < bossWheelPositions.Count - 1) {
                targetPosition = bossWheelPositions[i + 1].position;
            }
            else if (i >= bossWheelPositions.Count) {
                targetPosition = bossWheelPositions[0].position;
            }

            bossList[i].transform.position = targetPosition;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (currentBoss != null)
            Gizmos.DrawCube(currentBoss.transform.position, Vector3.one * 0.3f);
    }

    public void TriggerRotation() {
        swapTriggered = true;
    }
}
