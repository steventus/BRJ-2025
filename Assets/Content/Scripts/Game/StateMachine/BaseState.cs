using UnityEngine;
public abstract class BaseState : MonoBehaviour 
{
    public bool isComplete { get; protected set; }
    float startTime;
    protected float timeElapsed => Time.time - startTime;

    public virtual void EnterState() {}
    public virtual void UpdateState() {
        RotatePlayerTowardsCurrentBoss();
    }
    public virtual void ExitState() {}

    public void Initialize() {
        isComplete = false;
        startTime = Time.time;
    }

    //Game specific 
    
    [Header("Game Components")]
    public MusicManager musicManager;
    public BossRotationController bossRotationControl;
    public Transform player;

    public Conductor conductor;
    public Metronome metronome;
    
    //NEED:
    //  CHART SPAWN && CONTROL
    //  PLAYER INPUT

    void RotatePlayerTowardsCurrentBoss() {
        Transform currentBoss = bossRotationControl.currentBoss.transform;
        var targetRotation = Quaternion.LookRotation(currentBoss.position - player.position);
        float rotSpeed = 10f;
        player.rotation = Quaternion.Lerp(player.rotation,targetRotation, rotSpeed * Time.deltaTime);
    }
}
