using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnObject
{
    public GameObject enemy;

    public float timeStart;
}

public class SpawnerManager : MonoBehaviour
{
    public SpawnObject[] enemies;
    public Transform[] spawnPoints;

    public float spawnDelay;

    public float minAmt;
    public float maxAmt;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        for (int i = 0; i < Random.Range(minAmt, maxAmt); i++)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)].enemy, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        }
        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(Spawn());
    }
}
