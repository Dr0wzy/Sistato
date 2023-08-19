using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Wave
    {
        public Enemy[] enemies;
        public int count;
        public float timeBtwSpawn;
    }

    [SerializeField] Wave[] waves;

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float timeBtwWaves;

    Wave currentWave;
    int currentWaveIndex;
    Transform player;

    bool isSpawnFinished = false;
    [SerializeField] TextMeshProUGUI waveText;
    bool isFreeTime = true;
    float curtimeBtwWaves;

    void Start()
    {
        curtimeBtwWaves = timeBtwWaves;
        player = Player.instance.transform;
        StartCoroutine(CallNextWave(currentWaveIndex));
        UpdateText();
    }

    void Update()
    {
        UpdateText();
        if (isSpawnFinished && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            isSpawnFinished = false;
            if (currentWaveIndex + 1 < waves.Length)
            {
                currentWaveIndex++;
                StartCoroutine(CallNextWave(currentWaveIndex));
            }
        }
    }

    IEnumerator CallNextWave(int waveIndex)
    {
        isFreeTime = true;
        yield return new WaitForSeconds(timeBtwWaves);
        isFreeTime = false;
        StartCoroutine(Spawner(waveIndex));
    }

    IEnumerator Spawner(int waveIndex)
    {
        currentWave = waves[waveIndex];
        for (int i = 0; i < currentWave.count; i++)
        {
            if (player == null) yield break;
            Enemy randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(randomEnemy, randomSpawnPoint.position, Quaternion.identity);

            if (i == currentWave.count - 1)
            {
                isSpawnFinished = true;
            }

            else
            {
                isSpawnFinished = false;
            }

            yield return new WaitForSeconds(currentWave.timeBtwSpawn);
        }
    }

    void UpdateText()
    {
        if (isFreeTime) waveText.text = ((int)(curtimeBtwWaves -= Time.deltaTime)).ToString() + " until next wave";
        else waveText.text = "Current wave : " + (currentWaveIndex + 1).ToString();
    }

    // Fix the time not being reset
}
