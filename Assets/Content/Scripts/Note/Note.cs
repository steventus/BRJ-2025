using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public bool isPicked;
    public bool isStart, isEnd;

    public bool isInTrack;
    public bool isSelected;
    public bool isConnected;

    public bool isOccupied = false;
    public int noteIndex;
    public int connectedNoteIndex;

}
