using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;

    public float timeBetweenWaves = 6f;
    private float countdown = 2f;
    public TMPro.TextMeshProUGUI waveCountdownText;

    private int waveIndex = 0;
    private bool spawned = false;


    void Update ()
    {
        if (countdown <= 1f)
        {
            if (spawned == false)
            {
                StartCoroutine(SpawnWave());
                spawned = true;
            }
            if (countdown <= 0f)
            {
                countdown = timeBetweenWaves;
                spawned = false;
            }
        }

        countdown -= Time.deltaTime;

        waveCountdownText.text = Mathf.Floor(countdown).ToString();
    }

    IEnumerator SpawnWave ()
    {
        waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.3f);
        }

    }

    void SpawnEnemy ()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
