using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneTransition : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    AnimationClip openSceneClip;
    [SerializeField]
    AnimationClip closeSceneClip;
    public UnityEvent OnSceneOpenComplete;
    public UnityEvent OnSceneCloseComplete;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Start() {
        //open scene animation plays at start by default, call event at end of that clip
        StartCoroutine(CallEventAtEndOfClip(openSceneClip.length, OnSceneOpenComplete));
    }

    public void OpenScene() {
        animator.Play(openSceneClip.name);

        StartCoroutine(CallEventAtEndOfClip(openSceneClip.length, OnSceneOpenComplete));
    }
    public void CloseScene() {
        animator.Play(closeSceneClip.name);

        StartCoroutine(CallEventAtEndOfClip(closeSceneClip.length, OnSceneCloseComplete));
    }

    IEnumerator CallEventAtEndOfClip(float wait, UnityEvent _event) {
        yield return new WaitForSeconds(wait);
        _event?.Invoke();
    }
}
