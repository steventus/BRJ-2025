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

    [Header("TURN UI DISPLAY COLORS")]
    public Color enemyColor;
    public Color playerColor;

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
        string textToDisplay = "";

        if (stateMachine.state == enemyTurn)
        {
            textToDisplay = "STOP";
            stateUIDisplay.color = enemyColor;
        }
        else
        {
            textToDisplay = "GO!";
            stateUIDisplay.color = playerColor;
        }

        stateUIDisplay.text = textToDisplay;
    }

    //Debug Menus, won't appear in final builds
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 300, 220), "Debug Menu");
        GUI.Label(new Rect(20, 30, 300, 90), "Metronome position: " + Conductor.instance.loopBeatIndexPositionAnalog.ToString());
        GUI.Label(new Rect(20, 60, 150, 90), "Conductor beat index: " + Conductor.instance.loopBeatIndexPosition.ToString());
        GUI.Label(new Rect(20, 45, 150, 90), "Metronome target beat: " + Metronome.instance.beatIndexSelector);

        //if (GUI.Button(new Rect(20, 80, 100, 20), "End Phrase"))
        //{
        //    Events.PhraseEnded.Invoke();
        //}
        if (GUI.Button(new Rect(20, 100, 100, 20), "Heal Player"))
        {
            FindObjectOfType<PlayerHealthUi>().Heal(100);
        }
        //
        if (GUI.Button(new Rect(20, 120, 100, 20), "Damage Boss 20"))
        {
            Events.OnSuccessfulNoteHit.Invoke(20);
        }
        //if (GUI.Button(new Rect(20, 140, 100, 20), "WIN"))
        //{
        //    GetComponent<WinLose>().TriggerWin();
        //}
        //
        //if (GUI.Button(new Rect(20, 180, 100, 20), "LOSE"))
        //{
        //    GetComponent<WinLose>().TriggerLose();
        //}

        // if (GUI.Button(new Rect(20, 120, 100, 20), "Scratch CW"))
        // {
        //     Metronome.instance.currentNote.OnScratch(ScratchDirection.Direction.CW);
        // }

        // if (GUI.Button(new Rect(20, 140, 100, 20), "Scratch ACW"))
        // {
        //     Metronome.instance.currentNote.OnScratch(ScratchDirection.Direction.ACW);
        // }

        // if (GUI.Button(new Rect(20, 180, 100, 20), "Set Never Rotate Boss"))
        // {
        //     bool _debugNeverRotate = FindObjectOfType<EnemyTurn>().debugNeverRotate;
        //     FindObjectOfType<EnemyTurn>().debugNeverRotate = !_debugNeverRotate;
        // }

        // if (GUI.Button(new Rect(20, 200, 100, 20), "Start Current Music at loop"))
        // {
        //     Events.PhraseEnded.Invoke();
        //     FindObjectOfType<MusicManager>().DebugStartAtLoop();
        // }
    }
}

public static class Events
{
    public static UnityAction<int> OnSuccessfulNoteHit;
    public static UnityAction<int> OnUnsuccessfulNoteHit;
    public static UnityAction<int> OnBadNoteHit;

    //Input Events
    public static UnityAction PhraseEnded;
    public static UnityAction BarEnded;

}