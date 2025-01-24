using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;

public class Game : MonoBehaviour
{
    public StateMachine stateMachine;
    public EnemyTurn enemyTurn;
    public PlayerTurn playerTurn;

    [Header("Game Components")]
    public TextMeshProUGUI stateUIDisplay;
    private TrackFactory trackFactory = TrackFactory.instance;

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

    //Debug Menus, won't appear in final builds
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 150, 90), "Debug Menu");

        if (GUI.Button(new Rect(20, 40, 100, 20), "Rotate Boss"))
        {
            stateMachine.SetState(stateMachine.state == playerTurn ? enemyTurn : playerTurn);
        }
    }
}

public static class Events
{
    public static UnityAction<int> OnSuccessfulNoteHit;
    public static UnityAction<int> OnUnsuccessfulNoteHit;
    public static UnityAction<int> OnBadNoteHit;

    //Input Events
    public static UnityAction OnPhraseEnded;

}