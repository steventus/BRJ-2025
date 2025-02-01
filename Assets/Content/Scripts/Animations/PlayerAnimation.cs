using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Transform playerHand;
    RaycastHit hit;
    
    public GameObject hitSprite;
    void OnEnable() 
    {
    }
    void OnDisable()
    {
    }
    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool testRay = Physics.Raycast(ray, out hit, Mathf.Infinity, TurntableManager.instance.whatIsDisc);
        
        if(testRay) 
        {
            Vector3 point = hit.point;
            playerHand.position = point;
        }

        //Feedback animation on click disk
        if(TurntableManager.instance.OnInputDown())
        {
            StartCoroutine(HitFX());
        }
    }

    IEnumerator HitFX()
    {
        hitSprite.SetActive(true);

        float hitDuration = 0.1f;
        yield return new WaitForSeconds(hitDuration);

        hitSprite.SetActive(false);
    }
}
