using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletTimeUI : MonoBehaviour
{
    [SerializeField] Image meter;
    void OnEnable() {
        Game.events.OnParry += Test;
        Game.events.OnBulletTimeStart += Test2;
    }
    void OnDisable() {
        Game.events.OnParry -= Test;
        Game.events.OnBulletTimeStart -= Test2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Test(float amount) {
        Debug.Log("parry successful!");
        meter.fillAmount += amount;
    }

    void Test2() {
        meter.fillAmount -= Time.deltaTime * 0.5f;
    }
}
