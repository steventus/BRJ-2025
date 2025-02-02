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
        if (TM.rotationDirection > 0)
        {
            img.enabled = true;
            img.sprite = CW;
            //CW
        }
        if(TM.rotationDirection < 0)
        {
            img.enabled = true;
            img.sprite = ACW;
            //ACW
        }
    }
}
