using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WinLose : MonoBehaviour
{
    BossBehaviour[] bosses;
    HealthSystem playerHealth;
    public GameObject losePanel, winPanel;

    

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

        if(deadBosses == 3 )
        {
            winPanel.SetActive(true);
            Time.timeScale = 0;
        }

        if(playerHealth.CurrentHealth <= 0)
        {
            losePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        //SceneManager.LoadScene(MAINMENUINDEX);
    }
}
