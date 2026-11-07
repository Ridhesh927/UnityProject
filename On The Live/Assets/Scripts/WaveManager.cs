
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Wave Settings")]
    public int startingEnemies = 3;
    public int extraEnemiesPerWave = 2;
    public float spawnInterval = 1f;
    public float delayBetweenWaves = 3f;
    public int totalWaves = 5;

    public int CurrentWave { get; private set; }
    public int Kills { get; private set; }
    public int EnemiesAlive { get; private set; }
    public bool LevelComplete { get; private set; }

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (CurrentWave < totalWaves)
        {
            CurrentWave++;

            int enemiesToSpawn =
                startingEnemies +
                (CurrentWave - 1) * extraEnemiesPerWave;

            Debug.Log("WAVE " + CurrentWave + " STARTED!");

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }

            // Wait until every enemy in this wave is defeated.
            while (EnemiesAlive > 0)
                yield return new WaitForSeconds(0.25f);

            Debug.Log("WAVE " + CurrentWave + " CLEARED!");

            if (CurrentWave < totalWaves)
            {
                yield return new WaitForSeconds(delayBetweenWaves);
            }
        }

        LevelComplete = true;
        Debug.Log("LEVEL 1 COMPLETE!");
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null ||
            spawnPoints == null ||
            spawnPoints.Length == 0)
        {
            Debug.LogError(
                "WaveManager: Assign the prefab and spawn points!");
            return;
        }

        Transform point =
            spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy =
            Instantiate(enemyPrefab, point.position, Quaternion.identity);

        enemy.GetComponent<EnemyHealth>()?.SetWaveManager(this);
        EnemiesAlive++;
    }

    public void RegisterKill()
    {
        Kills++;
        EnemiesAlive = Mathf.Max(0, EnemiesAlive - 1);
    }
}
