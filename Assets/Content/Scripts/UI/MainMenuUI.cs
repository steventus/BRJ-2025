using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MainMenuUI : MonoBehaviour
{
    public Transform[] scenePositions;
    Vector3 camTargetPosition;
    public float speed;

    [SerializeField] int index;

    [Header("Visuals")]
    // Menu Graphic showing which option camera is currently looking at
    public Transform selectedHighlighter;
    public Transform[] menuButtons;

    void Start() {
        camTargetPosition = scenePositions[0].position;
    }
    void Update() {
        Vector3 lerp = Vector3.Lerp(transform.position, camTargetPosition, speed * Time.deltaTime);
        lerp.z = -10;
        transform.position = lerp;

        selectedHighlighter.position = menuButtons[index].position;
    }

    public void SetIndex(int _index) {
        index = _index;
        camTargetPosition = scenePositions[index].position;
    }

    public void QuitGame() {
        Application.Quit();
        //EditorApplication.ExitPlaymode();
    }
}
