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
    [SerializeField] Chart emptyChart;
    [HideInInspector] public bool debugNeverRotate = false;

    [Header("Variables")]
    //rotation check
    [SerializeField] float stateDuration;
    [SerializeField] int minAttacksBeforeRotation = 0;

    public override void EnterState()
    {
        //Debug.Log("enter " + transform.name);

        // [[ ENEMY START PHASE ]]
        if (rotateBoss && !debugNeverRotate)
        {
            musicManager.StartFade();
            bossRotationControl.RotateNextBoss(bossRotationControl.currentBoss);

            //TODO: Add Introduction phase here for first-time introductions

            rotateBoss = false;
        }

        // [[ TRANSITION PHASE ]]

        // intialize data needed for transition checks (boss health, attack counter)
        BossBehaviour _currentBoss = bossRotationControl.currentBoss;
        HealthSystem currentBossHealth = _currentBoss.Health;

        // boss state parameters
        //bool hasLostEnoughHealth = currentBossHealth.CurrentHealth <= _currentBoss.HealthThreshold;
        bool hasPerformedEnoughAttacks = _currentBoss.phrasesCompleted >= minAttacksBeforeRotation;
        bool isReadyToTransition = _currentBoss.ifReadyToTransition;

        //Update which boss' health is being displayed
        BossHealthUi bossHealthUi = healthSlider.GetComponent<BossHealthUi>();
        bossHealthUi.SetHealthComponent(currentBossHealth);
        bossHealthUi.OnBossChanged();

        // [[ ATTACK PHASE ]]
        Chart _chartToSpawn;

        //Check and perform boss transition
        if (!_currentBoss.IsPhaseTwo && _currentBoss.phrasesCompleted == 0 && isReadyToTransition)
        {
            //Play Transition Animations
            _currentBoss.PhaseTransition();
            Debug.Log("Phase Transition activate!");

            //To make up for additional phase required to accomodate phase transition
            _currentBoss.phrasesCompleted--;

            _chartToSpawn = emptyChart;
        }

        //Otherwise nothing
        else
        {
            //Transition boss if in phase 1 and has lost enough health, or has performedenoughattacks at any phase
            if (!_currentBoss.IsPhaseTwo && isReadyToTransition || hasPerformedEnoughAttacks)
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
        }

        currentChart = _chartToSpawn;
        TrackFactory.instance.CreateTrack(currentChart);
        conductor.beatsPerLoop = currentChart.notes.Count;

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

        //Reset old boss stats
        if (rotateBoss)
            bossRotationControl.currentBoss.phrasesCompleted = 0;


        inAttackPhase = false;
        attackComplete = false;


        //conductor.completedLoops = 0;
    }

    private void HandleBossDanceDue()
    {
        IPlayerInteractable _note = metronome.currentBeat.GetComponent<IPlayerInteractable>();
        NoteType.Note _noteType = _note.GetNoteType();

        IScratchDirection _noteDirection = metronome.currentBeat.GetComponent<IScratchDirection>();
        ScratchDirection.Direction _direction;
        if (_noteDirection != null)
            _direction = _noteDirection.GetScratchDirection();

        else _direction = ScratchDirection.Direction.CW;

        bossRotationControl.currentBoss.GetComponent<BossPresenter>().CheckNoteType(_noteType, _direction);
    }

    protected override void OnPhraseEnded()
    {
        if (inAttackPhase)
        {
            bossRotationControl.currentBoss.phrasesCompleted++;
        }
        base.OnPhraseEnded();
    }
}
