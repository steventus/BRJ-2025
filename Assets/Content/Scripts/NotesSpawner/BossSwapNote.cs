using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwapNote : MonoBehaviour
{
    BossRotationController bossRotater;
    void Awake() {
        bossRotater = FindObjectOfType<BossRotationController>();
    }
    public void SwapBosses() {
        bossRotater.TriggerRotation();
    }
}
