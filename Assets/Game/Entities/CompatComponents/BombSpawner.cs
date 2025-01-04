using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BombSpawner : MonoBehaviour
{
    //spawn damage zones on screen that explode after a set timer
    public List<Transform> spawnPoints = new List<Transform>();
    public int spawnedBombsCount { get; private set; }
    [SerializeField] Transform bombPrefab;
    [SerializeField] float delayBetweenBombSpawn;

    public void SpawnBomb() {
        Debug.Log("spawning bombs");
        StartCoroutine(PlaceBomb());
    }

    IEnumerator PlaceBomb() {
        while(spawnedBombsCount < spawnPoints.Count) {
            Transform bomb = Instantiate(bombPrefab, spawnPoints[spawnedBombsCount].position, Quaternion.identity);
            spawnedBombsCount++;
            yield return new WaitForSeconds(delayBetweenBombSpawn);
        }
    }

    public void ResetBombCount() {
        Debug.Log("reseting bomb spawner");
        spawnedBombsCount = 0;
    }
}