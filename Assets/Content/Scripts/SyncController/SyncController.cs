using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// To be attached to any classes that are performing a general action that needs to be timed to the Conductor.
/// When an action is queued, it will, by default, trigger on the next immediate beat based on the song BPM.
/// Multiple actions can be queued at once, and they will run on the next beat.
/// If multiple actions are queued rapidly, they will be queued onto the SAME next beat.
/// </summary>

public class QueueSyncedAction
{
    public UnityAction actionToRun;
    public float startTime;
    public float endTime;

    public QueueSyncedAction(UnityAction _action, float _beatDelay, float _startTime)
    {
        actionToRun = _action;
        endTime = _beatDelay;
        startTime = _startTime;
    }
}

public class SyncController : MonoBehaviour
{
    private List<Coroutine> queueActions = new List<Coroutine>();

    

    public void QueueForBeat(UnityAction _unityAction, int _beatDelay = 1)
    {
        float _startTime = Mathf.FloorToInt(Conductor.instance.songPositionInBeats);
        float _endTime = _startTime + _beatDelay;

        QueueSyncedAction _actionToRun = new QueueSyncedAction(_unityAction, _endTime, _startTime);
        queueActions.Add(StartCoroutine(Queing(_actionToRun)));
    }

    IEnumerator Queing(QueueSyncedAction _queuedAction)
    {
        //Waiting until the exact beat
        while (Conductor.instance.songPositionInBeats <= _queuedAction.endTime)
        {
            yield return new WaitForEndOfFrame();
        }
        _queuedAction.actionToRun.Invoke();
        yield return null;
    }
}
