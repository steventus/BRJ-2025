using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthUi : HealthUi
{
    void Start()
    {
        //TODO: Add observer event to call OnBossChanged
    }
    void OnDestroy()
    {
        //TODO: Add observer event to connect call OnBossChanged
    }

    void UpdateBossHealthUi()
    {
        //Get current boss from Boss Rotation Controller

        //Get health system from Boss

        //Change current health
    }

    void OnBossChanged()
    {
        UpdateBossHealthUi();
    }
}
