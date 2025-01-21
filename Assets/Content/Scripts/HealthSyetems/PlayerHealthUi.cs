using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUi : HealthUi
{
    void OnEnable()
    {
        Events.OnUnsuccessfulNoteHit += Damage;
        Events.OnBadNoteHit += Damage;
    }
    void OnDisable()
    {
        Events.OnUnsuccessfulNoteHit -= Damage;
        Events.OnBadNoteHit -= Damage;
    }

    protected override void Start()
    {
        base.Start();
        Heal(health.MaxHealth);
    }
}
