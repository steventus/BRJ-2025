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
    public Camera camera;
    public Transform player;
    public TextMeshProUGUI stateUIDisplay;
    void Start()
    {
        stateMachine = new();
        stateMachine.SetState(enemyTurn);
    }
    void Update()
    {
        DetermineState();
        stateMachine.state.UpdateState();

        ChangeCameraView();
        DisplayState();
    }

    void DetermineState() {
        if(stateMachine.state.isComplete) {
            if(stateMachine.state == enemyTurn) {
                stateMachine.SetState(playerTurn);
            }
            else if(stateMachine.state == playerTurn) {
                stateMachine.SetState(enemyTurn);
            }
        }
    }

    void ChangeCameraView() {
        float targetRot = 0;
        if(stateMachine.state == enemyTurn) {
            targetRot = 20f;            
        }
        else if(stateMachine.state == playerTurn) {
            targetRot = 35f;            
        }

        camera.transform.rotation = Quaternion.Euler(targetRot, 0, 0);
    }

    void DisplayState() {
        stateUIDisplay.text = stateMachine.state.transform.name;
    }
}