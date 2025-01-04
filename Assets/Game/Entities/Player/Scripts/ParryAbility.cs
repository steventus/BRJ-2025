using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryAbility : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] float parryCooldown, parryRadius, bulletTimeMeterFillAmount;

    bool isParrying = false;
    bool hasParried = false;
    [SerializeField] GameObject parrySuccessParticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isParrying) {
            Collider2D bulletCol = Physics2D.OverlapCircle(transform.position, parryRadius);
            if(bulletCol && bulletCol.transform != transform) {
                if(!hasParried) {
                    Game.events.OnParry(bulletTimeMeterFillAmount); 
                    parrySuccessParticles.SetActive(true);               
                    hasParried = true;
                    StartCoroutine(ResetParry());
                }
            }
        }
    }

    public void Parry() {
        if(isParrying) {
            return;   
        }
        
        playerCollider.enabled = false;
        animator.Play("Parry");
    }

    public void StartParry() {
        isParrying = true;
    }
    public void EndParry() {
        isParrying = false;
        playerCollider.enabled = true;
    }

    IEnumerator ResetParry() {
        yield return new WaitForSeconds(parryCooldown);
        hasParried = false;
    }
}
