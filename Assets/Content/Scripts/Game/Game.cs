using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{
    public StateMachine stateMachine;
    public EnemyTurn enemyTurn;
    public PlayerTurn playerTurn;

    [Header("Game Components")]
    public TextMeshProUGUI stateUIDisplay;
    public BossRotationController bossRotationControl;
    public Transform player;
    void Start()
    {
        stateMachine = new();
        stateMachine.SetState(enemyTurn);
    }
    void Update()
    {
        DetermineState();
        stateMachine.state.UpdateState();

        DisplayState();

        RotatePlayerTowardsCurrentBoss();
    }

    void DetermineState() {
        if(stateMachine.state.isComplete) {
            if(stateMachine.state == enemyTurn) {
                stateMachine.SetState(playerTurn);
            }
            else if(stateMachine.state == playerTurn) {
                stateMachine.SetState(enemyTurn);
                bossRotationControl.TriggerRotation();
            }
        }
    }

    void DisplayState() {
        stateUIDisplay.text = stateMachine.state.transform.name;
    }

    void RotatePlayerTowardsCurrentBoss() {
        Transform currentBoss = bossRotationControl.currentBoss.transform;
        var targetRotation = Quaternion.LookRotation(currentBoss.position - player.position);
        float rotSpeed = 10f;
        player.rotation = Quaternion.Lerp(player.rotation,targetRotation, rotSpeed * Time.deltaTime);
    }
}