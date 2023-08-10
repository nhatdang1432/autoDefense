using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    public GameObject bossPrefab;
    public GameObject enemyPrefab;
    public float spawnDelay = 5f;
    public float waveSpawnDelay = 5f;
    public int numEnemies = 10;
    private int _wave = 0;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < numEnemies; i++)
        {
            Spawn();
            yield return new WaitForSeconds(spawnDelay);
        }
        _wave++;

        if (_wave > 1)
        {
            SpawnBoss();
            _wave = 0;
        }
        yield return new WaitForSeconds(waveSpawnDelay);
    }

    public void Spawn()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        NavMeshObstacle obstacle = enemy.GetComponent<NavMeshObstacle>();

        if (agent != null)
        {
            agent.enabled = false;
        }
        if (obstacle != null)
        {
            obstacle.enabled = false;
        }
    }

    public void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab, transform.position, Quaternion.identity);
        NavMeshAgent agent = boss.GetComponent<NavMeshAgent>();
        NavMeshObstacle obstacle = boss.GetComponent<NavMeshObstacle>();

        if (agent != null)
        {
            agent.enabled = false;
        }
        if (obstacle != null)
        {
            obstacle.enabled = false;
        }
    }
}
