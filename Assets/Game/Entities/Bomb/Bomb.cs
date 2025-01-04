using UnityEngine;

public class Bomb : MonoBehaviour
{
    //on spawn, start timer
    //display bomb's area of effect so player can dodge
    //explodes after timer passes bomb lifetime
    
    float timeSpawned;
    float timeElapsed => Time.time - timeSpawned;
    [SerializeField] float timeUntilExplosion;
    [SerializeField] float explosionRadius;
    
    void Start() {
        timeSpawned = Time.time;
    }
    void Update() {
        if(timeElapsed >= timeUntilExplosion) {
            Explode();
        }
    }

    void Explode() {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, explosionRadius);

        if(hit) {
            //check collider for health component and deal damage
        }
        Destroy(gameObject);
        //destroy self
    }
}