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

    [Header("Phases")]
    [SerializeField] bool rotateBoss = false;
    [SerializeField] bool isAttacking = false;

    [Header("Variables")]
    //rotation check
    [SerializeField] float minHealthThreshold = 0.5f;
    [SerializeField] int minAttacksBeforeRotation = 0;

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
        BossBehaviour _currentBoss =  bossRotationControl.currentBoss;
        HealthSystem currentBossHealth = _currentBoss.Health; 
        float currentHealthInPercent = currentBossHealth.CurrentHealth / currentBossHealth.MaxHealth;
        
        
        // [[ ATTACK PHASE ]]
        
        // var chosenChart  
        
        bool hasLostEnoughHealth = currentHealthInPercent <= phaseThreshold;
        bool hasPerformedEnoughAttacks = _currentBoss.phrasesCompleted >= minAttacksBeforeRotation;  
        
        
        if (hasLostEnoughHealth || hasPerformedEnoughAttacks) {
            // store chart to spawn as a variable 
                // chosenChart = forcedRotateChart

            rotateBoss = true;
        }
        else {
            // choose a random chart attack
                // chosenChart = randomChart
        }

        //Spawn Chart Event

        // Initialize Chart - set position, spawn prefab notes
        // Enable Control of Notes Controller 
        isAttacking = true;
    }   
    
    public override void UpdateState() {
        RotatePlayerTowardsCurrentBoss();

        if(isAttacking) {
            // display chart && perform dance

            if(!isAttacking) {
                danceComplete = true;
            }
        }
        
        //check is boss is done dancing
        bool danceComplete = false;
        if(danceComplete) {
            isComplete = true;
        }
    }
    public override void ExitState() {
        Debug.Log("exit " + transform.name);

        isAttacking = false;
    }

    void RotatePlayerTowardsCurrentBoss() {
        Transform currentBoss = bossRotationControl.currentBoss.transform;
        var targetRotation = Quaternion.LookRotation(currentBoss.position - player.position);
        float rotSpeed = 10f;
        player.rotation = Quaternion.Lerp(player.rotation,targetRotation, rotSpeed * Time.deltaTime);
    }
}
