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

    void DisplayState() {
        stateUIDisplay.text = stateMachine.state.transform.name;
    }
}