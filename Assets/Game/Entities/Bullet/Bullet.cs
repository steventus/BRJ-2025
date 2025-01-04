using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] float timeSpawned, lifetime;
    
    void Start() {
        timeSpawned = Time.time;
    }
    void Update()
    {
        Move();
        CheckForHit();
        CheckLifetime();
    }

    private void CheckLifetime()
    {
        if(Time.time > (timeSpawned + lifetime)) {
            // gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void CheckForHit()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position + (transform.right*0.25f), 0.25f);
        if(hit && !hit.transform.TryGetComponent(out Bullet bullet)) {
            //when object pooling is implemented
                // gameObject.SetActive(false);
            
            Destroy(gameObject);

            if(hit.transform.TryGetComponent(out PlayerController player)) {

            }
        }
    }

    protected virtual void Move() {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    public void SetMoveSpeed(float speed) {
        moveSpeed = speed;
    }
}
