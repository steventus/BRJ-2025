using UnityEngine;
public abstract class BaseState : MonoBehaviour 
{
    protected Core core;
    public bool isComplete { get; protected set; }
    float startTime;
    protected float timeElapsed => Time.time - startTime;
    public virtual void EnterState() {}
    public virtual void UpdateState() {}
    public virtual void ExitState() {}

    public void SetCore(Core _core) {
        core = _core;
    }
    public void Initialize() {
        isComplete = false;
        startTime = Time.time;
    }
}
