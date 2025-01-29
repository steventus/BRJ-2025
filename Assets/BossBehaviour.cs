using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [Header("NAME")]
    public string bossName;
    [Header("HP")]
    [SerializeField] HealthSystem health;
    public HealthSystem Health => health;

    #region PhaseTransition
    [SerializeField] private int healthThreshold = 50;
    public int HealthThreshold => healthThreshold;
    public bool ifReadyToTransition { get; private set; }
    private bool isPhaseTwo = false;
    public bool IsPhaseTwo => isPhaseTwo;
    #endregion

    public bool isDead;
    [Header("Attack Stats")]
    public int phrasesCompleted = 0;
    public AudioSource musicSource;
    [Header("Charts")]
    public Chart triggerRotationChart;
    public Chart[] Phase1Charts;
    public Chart[] Phase2Charts;
    int songIndex;
    void OnEnable()
    {
        //musicSource = GetComponentInChildren<AudioSource>();
        Events.OnSuccessfulNoteHit += Damage;
        Events.OnBadNoteHit += Heal;
    }

    void OnDisable()
    {
        Events.OnSuccessfulNoteHit -= Damage;
        Events.OnBadNoteHit -= Heal;
    }
    void Start()
    {
        //intiate health    
        health?.HealthChangedEvent.AddListener(OnHealthChanged);
        health?.HealthChangedEvent.AddListener(CheckForPhaseTransition);
        health = GetComponent<HealthSystem>();
        Heal(health.MaxHealth);
        // Reset();

        ifReadyToTransition = false;
    }

    void OnDestroy()
    {
        health?.HealthChangedEvent.RemoveListener(OnHealthChanged);
        health?.HealthChangedEvent.RemoveListener(CheckForPhaseTransition);
    }
    public void Damage(int _amount)
    {
        if (FindObjectOfType<BossRotationController>().currentBoss != this)
            return;

        //Don't take anymore damage if still in phase 1 and health below thresholdf\
        if (!isPhaseTwo && health.CurrentHealth <= healthThreshold)
        {
            health.SetHealth(healthThreshold);
            Debug.Log("In Phase 2 and lost enough health");
        }
        else health.DecreaseHealth(_amount);

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

    // [[ CHART SPAWN ]]
    public Chart GetRandomChart()
    {
        Chart[] _selectedCharts = isPhaseTwo ? Phase2Charts : Phase1Charts;
        return _selectedCharts[Random.Range(0, _selectedCharts.Length)];
    }

    // BOSS TRANSITION
    private void CheckForPhaseTransition()
    {
        if (!isPhaseTwo && health.CurrentHealth <= healthThreshold)
            ifReadyToTransition = true;
        else
            ifReadyToTransition = false;
    }

    public void PhaseTransition()
    {
        isPhaseTwo = true;
    }
}
