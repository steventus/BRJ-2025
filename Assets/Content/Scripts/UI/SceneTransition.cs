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
    public UnityEvent OnSceneCloseComplete;
    void Awake() {
        animator = GetComponent<Animator>();
    }

    public void OpenScene() {
        animator.Play(openSceneClip.name);
    }
    public void CloseScene() {
        animator.Play(closeSceneClip.name);

        StartCoroutine(CallEvent(closeSceneClip.length));
    }

    IEnumerator CallEvent(float wait) {
        yield return new WaitForSeconds(wait);
        OnSceneCloseComplete?.Invoke();
    }
}
