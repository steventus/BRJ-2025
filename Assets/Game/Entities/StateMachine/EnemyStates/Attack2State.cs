using UnityEngine;

public class Attack2State : BaseState 
{
    [SerializeField] BombSpawner bombSpawner;
    public override void EnterState() {
        Debug.Log("attack 2");
        bombSpawner.ResetBombCount();
        bombSpawner.SpawnBomb();
    }
    public override void UpdateState() {
        if(bombSpawner.spawnedBombsCount >= bombSpawner.spawnPoints.Count) {
            isComplete = true;
        }
    }
    public override void ExitState() {
    }
}