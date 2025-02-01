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
                isPaused = true;
                PauseGame();
            }
            else
            {
                isPaused = false;
                UnpauseGame();
            }
        }
    }

    public void PauseGame() 
    {
        Time.timeScale = 0;
        OnPause?.Invoke();
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1;
        OnUnpause?.Invoke();
    }
}
