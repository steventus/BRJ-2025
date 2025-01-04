using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinAttack : MonoBehaviour
{
    private List<KeyCode> circleInputSequence = new List<KeyCode> {
        KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.LeftArrow
    };
    int circleInputIndex = 0;
    public int completedCircleInputs { get; private set; }
    [SerializeField] GameObject hitParticles;
    [SerializeField] float timeBetweenAttacks = 0.25f;
    
    [SerializeField] float attackRadius = 1.5f;
    [SerializeField] Animator animator;
    [SerializeField] Image spinAttackRadial;
    bool isAttacking = false;

    public void ChargeSpin() {
        if(Input.GetKey(circleInputSequence[circleInputIndex])) {
            circleInputIndex++;    
            float spinVisualFill = (float)circleInputIndex / (float)circleInputSequence.Count;
            spinAttackRadial.fillAmount = spinVisualFill;    
        }

        if(circleInputIndex >= circleInputSequence.Count) {
            //reset list
            circleInputIndex = 0;

            //count each time you achieve a correct input combo
            completedCircleInputs++;
            Debug.Log(completedCircleInputs);
        }

       
    }
    public void UnleashSpin() {
        if(!isAttacking)
        //launch attacks based on how many rotations you successfully inputted
            StartCoroutine(Attack(completedCircleInputs));
    }

    IEnumerator Attack(int attacks) {
        animator.Play("Attack");
        int attacksCompleted = 0;
        isAttacking = true;

        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        while(attacksCompleted < attacks) {
            foreach(Collider2D target in targets) {
                Transform _target = target.transform;
                if(_target.TryGetComponent(out Enemy enemy)) {
                    Debug.Log(_target.name + " " + attacksCompleted);
                    hitParticles.transform.position = _target.position; 
                    hitParticles.SetActive(true);

                    float hitFlashDuration = 0.15f;
                    _target.localScale = new Vector3(1.25f,1.25f,0);
                    yield return new WaitForSeconds(hitFlashDuration);
                    hitParticles.SetActive(false);
                    _target.localScale = new Vector3(1f,1f,0);
                }
            }
            yield return new WaitForSeconds(timeBetweenAttacks);
            attacksCompleted++;
        }
        completedCircleInputs = 0;
        isAttacking = false;
        spinAttackRadial.fillAmount = 0;
    }    
}