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

    //unit type using health system
    public enum UnitType { Player, Boss }
    public UnitType unitType;
    void OnEnable() 
    {
        if(unitType == UnitType.Player)
        {
            Events.OnUnsuccessfulNoteHit += DecreaseHealth;
            Events.OnBadNoteHit += DecreaseHealth;
        }
    }
    void OnDisable() 
    {
        if(unitType == UnitType.Player)
        {
            Events.OnUnsuccessfulNoteHit += DecreaseHealth;
            Events.OnBadNoteHit += DecreaseHealth;
        }
    }
    void Start() 
    {
        currentHealth = MaxHealth;
    }
    public void SetHealth(int _number){
        currentHealth = _number;
        UpdateHealth();
    }

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
        //Debug.Log(name + " " +currentHealth);
        HealthChangedEvent.Invoke();
    }
}
