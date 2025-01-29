using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour
{
    [SerializeField] protected HealthSystem health;
    [SerializeField] protected Slider slider;

    [SerializeField] protected Image portrait;
    public Color startColor;

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

        // Test
        Debug.Log(name);
        StartCoroutine(DamageReaction());
    }

    IEnumerator DamageReaction() {

    // portrait reacts to taking damage
    // ================================================================

        portrait.color = Color.red;
        float timeOfDamage = Time.time;
        float colorChangeDuration = 1f;
        float _speed = 5f;
        
        while(Time.time < timeOfDamage + colorChangeDuration) 
        {
            portrait.color = Color.Lerp(portrait.color, startColor, Time.deltaTime * _speed);
            yield return null;
        }

        portrait.color = startColor;
        // change to hurt sprite
        // shake animation
        // change color to red, then fade back to white
        // change back to idle sprite

    // ================================================================

    }

    public void Heal(int _amount)
    {
        health.IncreaseHealth(_amount);
    }

    protected void UpdateView()
    {
        slider.value = (float)health.CurrentHealth / (float)health.MaxHealth;
        //Debug.Log(health.CurrentHealth);
    }

    protected void OnHealthChanged()
    {
        UpdateView();
    }

    public void SetHealthComponent(HealthSystem _health) {
        health = _health;
    }
}
