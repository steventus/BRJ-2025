using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Game : MonoBehaviour
{
    public StateMachine stateMachine;
    public EnemyTurn enemyTurn;
    public PlayerTurn playerTurn;

    [Header("Game Components")]
    public TextMeshProUGUI stateUIDisplay;
    private TrackFactory trackFactory = TrackFactory.instance;
    public JudgementLineBehaviour judgementLineBehaviour;

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

        #region TO BE REMOVED
        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.SetState(stateMachine.state == playerTurn ? enemyTurn : playerTurn);
        }
        #endregion
    }

    void DetermineState()
    {
        if (stateMachine.state.isComplete)
        {
            if (stateMachine.state == enemyTurn)
            {
                stateMachine.SetState(playerTurn);
            }
            else if (stateMachine.state == playerTurn)
            {
                stateMachine.SetState(enemyTurn);
            }
        }
    }

    void DisplayState()
    {
        stateUIDisplay.text = stateMachine.state.transform.name;
    }
}

public static class Events
{
    public static UnityAction<int> OnSuccessfulNoteHit;
    public static UnityAction<int> OnUnsuccessfulNoteHit;
    public static UnityAction<int> OnBadNoteHit;

    //Input Events
    public static string InputDown = "InputDown";
    public static string InputScratch = "InputScratch";
    public static string InputUp = "InputUp";

}