using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    public int MinHealth = 0;
    public int MaxHealth = 100;
    public int CurrentHealth => currentHealth;
    private int currentHealth;

    public UnityEvent HealthChangedEvent = new UnityEvent();

    public void IncreaseHealth(int _delta)
    {
        currentHealth += _delta;
        currentHealth = Mathf.Clamp(currentHealth, MinHealth, MaxHealth);
        UpdateHealth();
    }

    public void DecreaseHealth(int _delta)
    {
        currentHealth -= _delta;
        currentHealth = Mathf.Clamp(currentHealth, MinHealth, MaxHealth);
        UpdateHealth();
    }

    void UpdateHealth()
    {
        HealthChangedEvent.Invoke();
    }
}
