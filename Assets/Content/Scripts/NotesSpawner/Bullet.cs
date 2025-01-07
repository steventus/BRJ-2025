using System;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] float timeSpawned, lifetime;

    [SerializeField] UnityEvent OnHit;
    
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
        if(hit) {
            //when object pooling is implemented
                // gameObject.SetActive(false);
            
            OnHit?.Invoke();
            transform.localScale = (Vector3.one * 1.35f);
        }
    }

    protected virtual void Move() {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    public void SetMoveSpeed(float speed) {
        moveSpeed = speed;
    }
}
