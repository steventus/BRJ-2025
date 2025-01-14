using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BossRotationController : MonoBehaviour
{
    public List<BossBehaviour> bossList;
    private Queue<BossBehaviour> activeBossList = new Queue<BossBehaviour>();
    public BossBehaviour currentBoss { get; private set; }
    [SerializeField] bool swapTriggered = false;
    public UnityEvent BossChangedEvent = new UnityEvent();

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

        //TEST --- TO BE REMOVED AFTERWARDS
        if (Input.GetKeyDown(KeyCode.P)){
            currentBoss.Damage(10);
        }

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
        UpdateBossChange();
        return _nextBoss;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (currentBoss != null)
            Gizmos.DrawCube(currentBoss.transform.position, Vector3.one * 0.3f);
    }

    public void UpdateBossChange()
    {
        BossChangedEvent.Invoke();
    }

    public void TriggerRotation()
    {
        swapTriggered = true;
    }
}
