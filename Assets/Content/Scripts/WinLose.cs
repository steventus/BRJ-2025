using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class WinLose : MonoBehaviour
{
    BossBehaviour[] bosses;
    HealthSystem playerHealth;
    public GameObject losePanel, winPanel;

    public UnityEvent OnLose;
    public UnityEvent OnWin;
    private void Awake()
    {
        losePanel.SetActive(false);
        winPanel.SetActive(false);
    }
    private void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<HealthSystem>();
        bosses = FindObjectsOfType<BossBehaviour>();
    }

    private void Update()
    {
        int deadBosses = 0;

        foreach (BossBehaviour b in bosses)
        {
            if (b.Health.CurrentHealth <= 0)
            {
                deadBosses++;
            }
        }

        if (deadBosses == 3)
        {
            OnWin?.Invoke();
        }

        if (playerHealth.CurrentHealth <= 0)
        {
            OnLose?.Invoke();
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    //DEBUG 

    public void TriggerWin() 
    {
            OnWin?.Invoke();

    }
    public void TriggerLose()
    {
            OnLose?.Invoke();

    }
}
