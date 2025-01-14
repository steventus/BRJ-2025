using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour
{
    [SerializeField] protected HealthSystem health;
    [SerializeField] protected Slider slider;
    protected virtual void Start()
    {
        if (health != null)
        {
            health.HealthChangedEvent.AddListener(OnHealthChanged);
        }
    }

    protected virtual void OnDestroy()
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

    protected void UpdateView()
    {
        slider.value = (float)health.CurrentHealth / (float)health.MaxHealth;
        Debug.Log(health.CurrentHealth);
    }

    protected void OnHealthChanged()
    {
        UpdateView();
    }
}
