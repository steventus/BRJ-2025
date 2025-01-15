using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : BaseState
{
    [SerializeField] float stateDuration;

    [Header("Phases")]
    [SerializeField] bool rotateBoss = false;
    [SerializeField] bool inAttackPhase = false;
    [SerializeField] bool attackComplete = false;

    [Header("Variables")]
    //rotation check
    [SerializeField] float minHealthThreshold = 0.5f;
    [SerializeField] int minAttacksBeforeRotation = 0;
    
    public override void EnterState() {
        Debug.Log("enter " + transform.name);

        // [[ ENEMY START PHASE ]]
        if(rotateBoss) {
            
            musicManager.StartFade();
            bossRotationControl.TriggerRotation();
            Debug.Log("rotate");

            rotateBoss = false;
        }

    // [[ TRANSITION PHASE ]]

        // intialize data needed for transition checks (boss health, attack counter)
        BossBehaviour _currentBoss = bossRotationControl.currentBoss;
        HealthSystem currentBossHealth = _currentBoss.Health; 
        float currentHealthInPercent = currentBossHealth.CurrentHealth / currentBossHealth.MaxHealth;
        bool hasLostEnoughHealth = currentHealthInPercent <= minHealthThreshold;
        bool hasPerformedEnoughAttacks = _currentBoss.phrasesCompleted >= minAttacksBeforeRotation;  
        
        // determine which chart to spawn
        //var chosenChart;  
        
        if (hasLostEnoughHealth || hasPerformedEnoughAttacks) {
            // store chart to spawn as a variable 
                // chosenChart = forcedRotateChart

            rotateBoss = true;
        }
        else {
            // choose a random chart attack
                // chosenChart = randomChart
        }

    // [[ ATTACK PHASE ]]

        //Spawn Chart Event
        // boss.spawnChart(chosenChart);

        // display chart && perform dance
        // boss.dance()

        inAttackPhase = true;
    }   
    
    public override void UpdateState() {
        base.UpdateState();

        if(inAttackPhase) {

            // if attack is complete
            if(attackComplete ) {
                // end enemy turn
                isComplete = true;
            }
        }
    }
    public override void ExitState() {
        Debug.Log("exit " + transform.name);

        inAttackPhase = false;
        attackComplete = false;
    }

}
