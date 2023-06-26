using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSceneManager : MonoBehaviour
{
    [Range(0, 4)] private int waveNumber = -1; 
    private Spawner[] spawners;
    public List<Wave> waves;
    public List<GameObject> EnemiesSpawned;

    private int enemiesToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        spawners = FindObjectsByType<Spawner>(FindObjectsSortMode.None);
        EnemiesSpawned = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        if(EnemiesSpawned.Count <= 0)
        {
            waveNumber++;
            if (waveNumber == 1)
            {
                AudioManager.Instance.ChangeMusic(MusicType.T2, 3, 0.15f);
            }

            Spawn();
        }

        if(waveNumber > waves.Count)
        {
            SceneManager.LoadScene(3);
        }
    }
    public void Spawn()
    {
        enemiesToSpawn = waves[waveNumber].StandardEnemiesToSpawn + waves[waveNumber].RangedEnemiesToSpawn;
        int standardSpawned = 0;
        int rangedSpawned = 0;
        for (int i = 0; i < enemiesToSpawn; i++)
        {

            while(standardSpawned < waves[waveNumber].StandardEnemiesToSpawn)
            {
                int randomIndex = Random.Range(0, spawners.Length);
                if (spawners[randomIndex].type == SpawnerType.Standard)
                {
                    float speedMultiplier = waves[waveNumber].EnemiesSpeedMultiplier;
                    float hpMultiplier = waves[waveNumber].EnemiesHpMultiplier;
                    spawners[randomIndex].SpawnEnemy(speedMultiplier, hpMultiplier);
                    standardSpawned++;
                }
            }
            while(rangedSpawned < waves[waveNumber].RangedEnemiesToSpawn)
            {
                int randomIndex = Random.Range(0, spawners.Length);
                if (spawners[randomIndex].type == SpawnerType.Ranged && rangedSpawned < waves[waveNumber].RangedEnemiesToSpawn)
                {
                    float speedMultiplier = waves[waveNumber].EnemiesSpeedMultiplier;
                    float hpMultiplier = waves[waveNumber].EnemiesHpMultiplier;
                    spawners[randomIndex].SpawnEnemy(speedMultiplier, hpMultiplier);
                    rangedSpawned++;
                }
            }
            
            
        }
    }
}
