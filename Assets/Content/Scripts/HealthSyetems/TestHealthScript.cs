using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public HealthUi health;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.Damage(10);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            health.Heal(10);
        }
    }
}
