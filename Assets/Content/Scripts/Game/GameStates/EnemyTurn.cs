using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : BaseState
{
    [SerializeField] float stateDuration;

    [Header("Game Components")]
    public MusicManager musicManager;
    public BossRotationController bossRotationControl;
    public Transform player;

    //rotation check
    [SerializeField] 
    bool rotateBoss = false;

    void OnEnable() {
        //subscribe state to event that triggers boss rotation
    }
    void OnDisable() {
        //unsubscribe state to event that triggers boss rotation
    }
    public override void EnterState() {
        Debug.Log("enter " + transform.name);

        // [[ ENEMY START PHASE ]]
        if(rotateBoss) {
            
            if(bossRotationControl.currentBoss == musicManager.bossBehaviours[musicManager.currentBossIndex])
                bossRotationControl.TriggerRotation();

            musicManager.StartFade();
            rotateBoss = false;
        }

        // [[ TRANSITION PHASE ]]
        HealthSystem currentBossHealth = bossRotationControl.currentBoss.Health; 
        float currentHealthInPercent = currentBossHealth.CurrentHealth / currentBossHealth.MaxHealth;
    }   
    public override void UpdateState() {
        RotatePlayerTowardsCurrentBoss();

        if(timeElapsed >= stateDuration) {
            isComplete = true;
        }
    }
    public override void ExitState() {
        Debug.Log("exit " + transform.name);
    }

    void RotatePlayerTowardsCurrentBoss() {
        Transform currentBoss = bossRotationControl.currentBoss.transform;
        var targetRotation = Quaternion.LookRotation(currentBoss.position - player.position);
        float rotSpeed = 10f;
        player.rotation = Quaternion.Lerp(player.rotation,targetRotation, rotSpeed * Time.deltaTime);
    }
}
