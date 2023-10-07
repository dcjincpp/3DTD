using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static int EnemiesAlive = 0;
    public WaveManager[] waves;
    public Transform spawnPoint;

    public float timeBetweenWaves = 3f;
    private float countdown = 2f;
    public TMPro.TextMeshProUGUI waveCountdownText;

    private int waveIndex = 0;


    public void StartNextWave ()
    {
        if(EnemiesAlive > 0)
        {
            return;
        }

        StartCoroutine(SpawnWave());

        // if (countdown <= 0f)
        // {
        //     StartCoroutine(SpawnWave());
        //     countdown = timeBetweenWaves;
        //     return;
        // }

        // countdown -= Time.deltaTime;

        // countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        // waveCountdownText.text = string.Format("{0:00.00}", countdown);
    }

    IEnumerator SpawnWave ()
    {
        PlayerResources.Rounds++;

        WaveManager wave = waves[waveIndex];

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            //how fast they spawn after the other
            yield return new WaitForSeconds(1f / wave.rate);
        }

        waveIndex++;

        if(waveIndex == waves.Length)
        {
            Debug.Log("Waves Complete");
            this.enabled = false;
        }
    }

    void SpawnEnemy (GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        EnemiesAlive++;
    }
}
