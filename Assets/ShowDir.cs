using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDir : MonoBehaviour
{
    private TurntableManager TM;
    public Image img;
    public Sprite CW, ACW;

    // Start is called before the first frame update
    void Start()
    {
        TM = GameObject.FindObjectOfType<TurntableManager>();
        img.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ScratchDirection.Direction _scratchInput = TurntableManager.instance.ScratchInput();
        if (_scratchInput == ScratchDirection.Direction.CW)
        {
            img.enabled = true;
            img.sprite = CW;
            //CW
        }
        else if(_scratchInput == ScratchDirection.Direction.ACW)
        {
            img.enabled = true;
            img.sprite = ACW;
            //ACW
        }

        else
        {
            img.enabled = false;
        }
    }
}
