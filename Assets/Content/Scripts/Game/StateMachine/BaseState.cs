using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public abstract class BaseState : MonoBehaviour
{
    public bool isComplete { get; protected set; }
    float startTime;
    protected float timeElapsed => Time.time - startTime;

    protected virtual void OnEnable()
    {
        Events.PhraseEnded += OnPhraseEnded;
    }
    protected virtual void OnDisable()
    {
        Events.PhraseEnded -= OnPhraseEnded;
    }
    public virtual void EnterState() { }
    public virtual void UpdateState()
    {
        RotatePlayerTowardsCurrentBoss();

        //Complete phase on end of chart

    }
    public virtual void ExitState() { }

    public void Initialize()
    {
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

    //Health Bar UI
    public HealthUi healthSlider;

    void RotatePlayerTowardsCurrentBoss()
    {
        Transform currentBoss = bossRotationControl.currentBoss.transform;
        var targetRotation = Quaternion.LookRotation(currentBoss.position - player.position);
        float rotSpeed = 10f;
        player.rotation = Quaternion.Lerp(player.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }

    protected virtual void OnPhraseEnded()
    {
        isComplete = true;
    }
}
