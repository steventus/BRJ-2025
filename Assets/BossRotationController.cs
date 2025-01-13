using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossRotationController : MonoBehaviour
{
    public List<BossBehaviour> bossList;
    private Queue<BossBehaviour> activeBossList = new Queue<BossBehaviour>();
    public BossBehaviour currentBoss { get; private set; }
    [SerializeField] bool swapTriggered = false;
    void Start()
    {
        for (int i = 0; i < bossList.Count; i++)
        {
            activeBossList.Enqueue(bossList[i]);
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

        currentBoss = _nextBoss;
        return _nextBoss;
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
