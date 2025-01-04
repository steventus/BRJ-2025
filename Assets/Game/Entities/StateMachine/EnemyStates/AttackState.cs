using UnityEngine;

public class AttackState : BaseState 
{
    [SerializeField] ProjectileShooter projectileShooter;
    [SerializeField] Transform[] waypoints;
    int currentWaypointIndex;
    [SerializeField] float speed;
    Vector3 dirToWaypoint;
    
    public override void EnterState() {
        Debug.Log("attack");
    }
    public override void UpdateState() {
        if(!projectileShooter.isShooting) {
            if (projectileShooter.burstsFired >= projectileShooter.burstCount){
                isComplete = true;
                return;
            }

            projectileShooter.Shoot();
        }
    }
    public override void ExitState() {
        projectileShooter.ResetBursts();
    }
}