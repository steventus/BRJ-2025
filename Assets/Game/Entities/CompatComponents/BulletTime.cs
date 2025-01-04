using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : MonoBehaviour
{
    [SerializeField] Camera camera;
    public float currentMeter { get; private set;}
    [SerializeField] float bulletTimeScale;
    bool isActive = false;
    void OnEnable() {
        Game.events.OnParry += IncreaseMeter;
    }
    void OnDisable() {
        Game.events.OnParry -= IncreaseMeter;
    }
    void Start() {
        camera = Camera.main;
    }
    void Update()
    {
        if(isActive && currentMeter > 0) {
            Time.timeScale = bulletTimeScale;

            currentMeter -= Time.deltaTime * 0.5f;
            Game.events.OnBulletTimeStart();

            camera.orthographicSize = 7.5f;
        }
        else {
            Time.timeScale = 1;
            camera.orthographicSize = 8f;
            Game.events.OnBulletTimeEnd();
        }
    }

    public void Activate() {
        isActive = true;
    }
    public void Deactivate() {
        isActive = false;
    }

    void IncreaseMeter(float amount) {
        currentMeter += amount;
    }
}
