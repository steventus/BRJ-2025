using System;
using UnityEngine;

public class Core : MonoBehaviour
{
    public StateMachine stateMachine;

    public Transform parent;

    public void SetUpStates() {
        stateMachine = new();

        BaseState[] states = GetComponentsInChildren<BaseState>();
        foreach(BaseState state in states) {
            state.SetCore(this);
        }
    }
}