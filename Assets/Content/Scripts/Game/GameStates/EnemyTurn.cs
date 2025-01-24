using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : BaseState
{
    [Header("References")]
    Chart currentChart;

    [Header("Phases")]
    [SerializeField] bool rotateBoss = false;
    [SerializeField] bool inAttackPhase = false;
    [SerializeField] bool attackComplete = false;

    [Header("Variables")]
    //rotation check
    [SerializeField] float stateDuration;
    [SerializeField] float minHealthThreshold = 0.5f;
    [SerializeField] int minAttacksBeforeRotation = 0;

    public override void EnterState()
    {
        Debug.Log("enter " + transform.name);

        // [[ ENEMY START PHASE ]]
        if (rotateBoss)
        {
            //Reset old boss stats
            bossRotationControl.currentBoss.phrasesCompleted = 0;

            musicManager.StartFade();
            bossRotationControl.TriggerRotation();

            Debug.Log("rotate");

            rotateBoss = false;
        }

        // [[ TRANSITION PHASE ]]

        // intialize data needed for transition checks (boss health, attack counter)
        BossBehaviour _currentBoss = bossRotationControl.currentBoss;
        HealthSystem currentBossHealth = _currentBoss.Health;

        //Update which boss' health is being displayed
        BossHealthUi bossHealthUi = healthSlider.GetComponent<BossHealthUi>();
        bossHealthUi.SetHealthComponent(currentBossHealth);
        bossHealthUi.OnBossChanged();

        // boss state parameters
        float currentHealthInPercent = currentBossHealth.CurrentHealth / currentBossHealth.MaxHealth;
        bool hasLostEnoughHealth = currentHealthInPercent <= minHealthThreshold;
        bool hasPerformedEnoughAttacks = _currentBoss.phrasesCompleted >= minAttacksBeforeRotation;

        // determine which chart to spawn
        // get list of possible charts from currentBoss

        // store chart to spawn as a variable   
        Chart _chartToSpawn;

        if (hasLostEnoughHealth || hasPerformedEnoughAttacks)
        {
            // chosenChart = forcedRotateChart
            rotateBoss = true;
            _chartToSpawn = _currentBoss.triggerRotationChart;
        }
        else
        {
            // choose a random chart attack
            _chartToSpawn = _currentBoss.GetRandomChart();
        }

        currentChart = _chartToSpawn;

        // [[ ATTACK PHASE ]]
        // INITIALIZE attack phase

        TrackFactory.instance.CreateTrack(currentChart);
        conductor.beatsPerLoop = currentChart.notes.Count;
        //start attack phase
        inAttackPhase = true;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (inAttackPhase)
        {
            //set positions for judgement line visual
            metronome.SetMarkers();

            //Boss Dance Presenter
            HandleBossDanceDue();
        }
    }
    public override void ExitState()
    {
        //Debug.Log("exit " + transform.name);

        inAttackPhase = false;
        attackComplete = false;


        //conductor.completedLoops = 0;
    }

    private void HandleBossDanceDue()
    {
        IPlayerInteractable _note = metronome.nextBeat.GetComponent<IPlayerInteractable>();
        NoteType.Note _noteType = _note.GetNoteType();

        IScratchDirection _noteDirection = metronome.nextBeat.GetComponent<IScratchDirection>();
        ScratchDirection.Direction _direction;
        if (_noteDirection != null)
            _direction = _noteDirection.GetScratchDirection();

        else _direction = ScratchDirection.Direction.CW;

        bossRotationControl.currentBoss.GetComponent<BossPresenter>().CheckNoteType(_noteType, _direction);
    }

    protected override void OnPhraseEnded()
    {
        if (inAttackPhase)
            bossRotationControl.currentBoss.phrasesCompleted++;
        base.OnPhraseEnded();
    }

}
