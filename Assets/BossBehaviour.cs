using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] HealthSystem health;
    public HealthSystem Health => health;
    public bool isDead;
    [Header("Attack Stats")]
    public int phrasesCompleted = 0;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioClip[] bossSongs;
    int songIndex;
    void OnEnable() {
        musicSource = GetComponentInChildren<AudioSource>();
    }
    void Start()
    {
        //intiate health    
        health?.HealthChangedEvent.AddListener(OnHealthChanged);
        health = GetComponent<HealthSystem>();
        Heal(health.MaxHealth);
        // Reset();
    }

    void OnDestroy()
    {
        health?.HealthChangedEvent.RemoveListener(OnHealthChanged);
    }
    public void Damage(int _amount)
    {
        health.DecreaseHealth(_amount);

    }

    public void Heal(int _amount)
    {
        health.IncreaseHealth(_amount);
    }

    public void Reset()
    {
        health.IncreaseHealth(health.MaxHealth);
    }

    void OnHealthChanged()
    {
        if (health.CurrentHealth <= 0)
        {
            isDead = true;
            Debug.Log(gameObject.name + " is dead.");
        }
    }
}
