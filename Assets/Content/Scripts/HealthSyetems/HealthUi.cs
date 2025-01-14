using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour
{
    [SerializeField] protected HealthSystem health;
    [SerializeField] Slider slider;
    void Start()
    {
        if (health != null)
        {
            health.HealthChangedEvent.AddListener(OnHealthChanged);
        }
    }

    void OnDestroy()
    {
        if (health != null)
        {
            health.HealthChangedEvent.RemoveListener(OnHealthChanged);
        }
    }

    public void Damage(int _amount)
    {
        health.DecreaseHealth(_amount);
    }

    public void Heal(int _amount)
    {
        health.IncreaseHealth(_amount);
    }

    void UpdateView()
    {
        slider.value = (float)health.CurrentHealth / (float)health.MaxHealth;
    }

    void OnHealthChanged()
    {
        UpdateView();
    }
}
