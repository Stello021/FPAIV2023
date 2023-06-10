using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    spawners[randomIndex].SpawnEnemy();
                    standardSpawned++;
                }
            }
            while(rangedSpawned < waves[waveNumber].RangedEnemiesToSpawn)
            {
                int randomIndex = Random.Range(0, spawners.Length);
                if (spawners[randomIndex].type == SpawnerType.Ranged && rangedSpawned < waves[waveNumber].RangedEnemiesToSpawn)
                {
                    spawners[randomIndex].SpawnEnemy();
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
}
