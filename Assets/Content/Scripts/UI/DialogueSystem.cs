using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public string[] dialogue;
    public float textSpeed;

    private int index;

    bool dialogueActive = true;

    public UnityEvent OnContinueDialogue;

    [Header("Dynamic Buttons")]
    public GameObject continueButton;
    public GameObject replyButton;
    void Start() {
        tmp.text = string.Empty;
        StartDialogue();
    }

    void Update() {
        if(index == dialogue.Length - 1 && tmp.text == dialogue[index]) {
            replyButton.SetActive(true);
        }

        if(Input.GetMouseButtonDown(0)) {
            continueButton.SetActive(false);
            ContinueDialogue();
        }
    }

    void StartDialogue() {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() {
        foreach(char c in dialogue[index].ToCharArray()) {
            tmp.text += c;
            yield return new WaitForSeconds(textSpeed);
            
            if(tmp.text == dialogue[index] && index < dialogue.Length - 1) {
                continueButton.SetActive(true);
            }
        }
    }

    void NextLine() {
        if(index < dialogue.Length - 1) {
            index++;
            tmp.text = string.Empty;
            StartCoroutine(TypeLine());
        }
    }

    public void ContinueDialogue() {
        if(tmp.text == dialogue[index]) {
            NextLine();
            OnContinueDialogue?.Invoke();
        }
        else {
            StopAllCoroutines();
            tmp.text = dialogue[index];
            continueButton.SetActive(true);
        }

    }
}
