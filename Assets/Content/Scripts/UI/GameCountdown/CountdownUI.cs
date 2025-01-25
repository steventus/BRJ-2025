using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CountdownUI : MonoBehaviour
{
    Animator animator;

    [Header("Animation Clips")]
    [SerializeField]
    AnimationClip BGEnterClip;
    [SerializeField]
    AnimationClip countClip;

    [Header("Events To Call")]
    public UnityEvent OnBGEntered;
    public UnityEvent OnCountComplete;
    public UnityEvent OnLastCount;

    [Header("Countdown")]
    [SerializeField] Transform[] numbers;
    [SerializeField] int countIndex = 0;
    
    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Start() {
        //open scene animation plays at start by default, call event at end of that clip
        StartCoroutine(CallEventAtEndOfClip(BGEnterClip.length, OnBGEntered));
    }

    public void StartCountdown() {
        //set & enable current nubmer visual ui
        for(int i = 0; i < numbers.Length; i++) {
            GameObject number = numbers[i].gameObject;
            
            if(i == countIndex) {
                number.SetActive(true);
            }
            else {
                number.SetActive(false);
            }
        }

        countIndex++;
        animator.Play(countClip.name);

        if(countIndex < numbers.Length) 
        {
            StartCoroutine(CallEventAtEndOfClip(countClip.length, OnCountComplete));
        }
        else
        {
            StartCoroutine(CallEventAtEndOfClip(countClip.length, OnLastCount));
        }
    } 

    IEnumerator CallEventAtEndOfClip(float wait, UnityEvent _event) {
        yield return new WaitForSeconds(wait);
        _event?.Invoke();
    }
}
