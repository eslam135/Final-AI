using System.Collections;
using UnityEngine;

public class ZombieWaveManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject baseZombiePrefab;
    public GameObject[] specialZombiePrefabs;

    [Header("Wave Settings")]
    public int startingZombies = 5;
    public float spawnCircleRadius = 3f;
    public float spawnInterval = 1f;
    public float timeBetweenWaves = 5f;

    [Header("Progression")]
    public float zombieIncreasePerWave = 1.2f;
    public float specialZombieChance = 0.1f;

    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(startAfterSeconds());
    }
    IEnumerator startAfterSeconds()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(StartNextWave());
    }
    IEnumerator StartNextWave()
    {
        currentWave++;
        int zombiesToSpawn = Mathf.RoundToInt(startingZombies * Mathf.Pow(zombieIncreasePerWave, currentWave - 1));

        for (int i = 0; i < zombiesToSpawn; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitForSeconds(timeBetweenWaves);

        StartCoroutine(StartNextWave());
    }

    void SpawnZombie()
    {
        float randomOffset = Random.Range(0.5f, 2f);
        Vector2 spawnCircle = Random.insideUnitCircle.normalized * (spawnCircleRadius + randomOffset);
        Vector3 spawnPos = player.position + new Vector3(spawnCircle.x, 0, spawnCircle.y);

        GameObject zombieToSpawn;
        if (Random.value < specialZombieChance && specialZombiePrefabs.Length > 0)
        {
            
            int index = Random.Range(0, specialZombiePrefabs.Length);
            Debug.Log(index);
            zombieToSpawn = specialZombiePrefabs[index];
        }
        else
        {
            zombieToSpawn = baseZombiePrefab;
        }

        Instantiate(zombieToSpawn, spawnPos, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, spawnCircleRadius);
        }
    }
}