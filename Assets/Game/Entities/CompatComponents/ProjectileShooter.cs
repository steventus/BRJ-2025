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

    public void Shoot()
    {
        StartCoroutine(BulletBurst());
    }

    private IEnumerator BulletBurst()
    {
        isShooting = true;

        for(int i = 0; i < burstCount; i++) {

            float angleStep = (burstEndAngle - burstStartAngle) / projectilesPerBurst;
            float angle = burstStartAngle;
            if(!fireClockwise)
                angle = burstEndAngle;
            
            for(int j = 0; j < projectilesPerBurst + 1; j++) {
                float bulletDirX = transform.position.x + (Mathf.Sin((angle * Mathf.PI) / 180f) * spawnPointMultiplier);
                float bulletDirY = transform.position.y + (Mathf.Cos((angle * Mathf.PI) / 180f) * spawnPointMultiplier);
                
                Vector3 bulletAngleVector = new(bulletDirX, bulletDirY);
                bulletDirection = bulletAngleVector - transform.position;

                //instantiate bullet && set bullet transform to correct angle
                Transform bulletTransform = Instantiate(bulletPrefab, bulletAngleVector, Quaternion.identity);
                bulletTransform.right = bulletDirection.normalized;

                if (bulletTransform.TryGetComponent(out Bullet bullet))
                {
                    bullet.SetMoveSpeed(bulletSpeed);
                }

                //increase angle for next bullet
                if(!fireClockwise) {
                    angle -= angleStep;
                }
                else {
                    angle += angleStep;
                }

                if(staggerBullets) {
                    yield return new WaitForSeconds(staggerDelay);
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