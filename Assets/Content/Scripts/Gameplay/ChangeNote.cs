using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeNote : MonoBehaviour, IPlayerInteractable
{
    public bool isHit = false;
    Game gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<Game>();
    }
    public void OnInputDown()
    {
        switch (Metronome.instance.CheckIfInputIsOnBeat())
        {
            case Metronome.HitType.perfect:
                Debug.Log("Perfect!");
                isHit = true;
                gameManager.stateMachine.SetState(gameManager.enemyTurn);

                break;

            case Metronome.HitType.good:
                Debug.Log("Correct!");
                isHit = true;
                gameManager.stateMachine.SetState(gameManager.enemyTurn);
                break;

            case Metronome.HitType.miss:
                OnMiss();
                break;
        }
    }

    public void OnInputUp()
    {

    }

    public void OnMiss()
    {
        isHit = true;
        Metronome.instance.MissHit();
        gameManager.stateMachine.SetState(gameManager.enemyTurn);
        Debug.Log("Bad Change!");
    }
}
