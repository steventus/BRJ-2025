using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossHealthUi : HealthUi
{
    BossBehaviour selectedBoss;
    BossRotationController thisController;

    [SerializeField] TextMeshProUGUI bossNameTmp;
    void OnEnable()
    {
        Events.OnSuccessfulNoteHit += Damage;   
    }
    void OnDisable()
    {
        Events.OnSuccessfulNoteHit -= Damage;   
    }
    void Awake()
    {
        thisController = FindObjectOfType<BossRotationController>();
    }
    protected override void Start()
    {
        //TODO: Add observer event to call OnBossChanged
        thisController.BossChangedEvent.AddListener(OnBossChanged);

        //Need to subscribe to only the active boss
        health.HealthChangedEvent.AddListener(OnHealthChanged);

    }

    protected override void OnDestroy()
    {
        //TODO: Add observer event to connect call OnBossChanged
        thisController.BossChangedEvent.RemoveListener(OnBossChanged);

        health.HealthChangedEvent.RemoveListener(OnHealthChanged);
    }

    void UpdateBossHealthUi()
    {
        //Unsubscribe from old boss
        health.HealthChangedEvent.RemoveListener(OnHealthChanged);

        //Get current boss from Boss Rotation Controller
        selectedBoss = FindObjectOfType<BossRotationController>().currentBoss;

        //Get health system from new selected Boss
        health = selectedBoss.GetComponent<HealthSystem>();

        //Subscribe to new selected boss
        health.HealthChangedEvent.AddListener(OnHealthChanged);

        // change portrait
        portrait.sprite = selectedBoss.GetComponent<BossPresenter>().UIPortraitSprite;

        // change boss name text
        bossNameTmp.text = selectedBoss.bossName;

        //Call for immediate update
        UpdateView();
    }

    public void OnBossChanged()
    {
        UpdateBossHealthUi();
    }
}
