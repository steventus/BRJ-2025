using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] HealthSystem health;
    public bool isDead;
    void Start()
    {
        health?.HealthChangedEvent.AddListener(OnHealthChanged);
        health = GetComponent<HealthSystem>();
        Reset();
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
