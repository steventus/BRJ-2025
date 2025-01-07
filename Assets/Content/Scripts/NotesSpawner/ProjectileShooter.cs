using System.Collections;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] Transform bulletPrefab;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] float spawnPointMultiplier;
    
    public bool isShooting { get; private set; }
    Vector3 bulletDirection;
    public float bulletSpeed;
    public float timeBetweenBursts, burstEndDelay;
    //set these properties to public so different attack states can assign custom angle
    public int burstCount; 
    public int burstsFired { get; private set; }
    public int projectilesPerBurst;

    public bool staggerBullets = false;
    public float staggerDelay;
    public bool fireClockwise = true;
    
    public float burstStartAngle, burstEndAngle;

    void Start() {
        Shoot();
    }
    public void Shoot()
    {
        StartCoroutine(BulletBurst());
    }

    private IEnumerator BulletBurst()
    {
        isShooting = true;

        for(int i = 0; i < burstCount; i++) {
            for(int j = 0; j < projectilesPerBurst + 1; j++) {
                //instantiate bullet
                Transform bulletTransform = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                if (bulletTransform.TryGetComponent(out Bullet bullet))
                {
                    bullet.SetMoveSpeed(bulletSpeed);
                }
            }
            
            burstsFired++;
            yield return new WaitForSeconds(timeBetweenBursts);
        }

        yield return new WaitForSeconds(burstEndDelay);
        isShooting = false;
    }

    public void ResetBursts() {
        burstsFired = 0;
    }
}