using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    void OnEnable() {
        Events.OnSuccessfulNoteHit += Scratch;
    }
    void OnDisable() {
        Events.OnSuccessfulNoteHit -= Scratch;
    }
    void Awake() {
        animator = GetComponent<Animator>();
    }
    void Scratch(int i) {
        animator.Play("Scratch");
    }
}
