using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pause : MonoBehaviour
{
    bool isPaused = false;
    public UnityEvent OnPause;
    public UnityEvent OnUnpause;
    void Update() 
    {
        //On Escape Input, pause game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }
    }

    public void PauseGame() 
    {
        isPaused = true;
        Time.timeScale = 0;
        OnPause?.Invoke();
    }
    public void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        OnUnpause?.Invoke();
    }
}
