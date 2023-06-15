using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSceneManager : MonoBehaviour
{
    [Range(0, 4)] private int waveNumber; 
    private Spawner[] spawners;
    public List<Wave> waves;
    private int enemiesToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        spawners = FindObjectsByType<Spawner>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void Destroy()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].DestroyEnemies();
        }
        waveNumber++;
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
