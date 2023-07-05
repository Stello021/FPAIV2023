using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSceneManager : MonoBehaviour
{
    [Range(0, 4)] private int waveNumber = -1; 
    private Spawner[] spawners;
    public List<Wave> waves;
    public List<GameObject> EnemiesSpawned;

    private int enemiesToSpawn;
    [SerializeField] TMP_Text waveNumberText;

    // Start is called before the first frame update
    void Start()
    {
        spawners = FindObjectsByType<Spawner>(FindObjectsSortMode.None);
        EnemiesSpawned = new List<GameObject>();
        waveNumberText.text = (waveNumber + 2).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (waveNumber > waves.Count)
        {
            SceneManager.LoadScene(3);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (EnemiesSpawned.Count <= 0)
        {
            waveNumber++;

            waveNumberText.text = (waveNumber + 1).ToString();

            if (waveNumber == 4)
            {
                AudioManager.Instance.ChangeMusic(MusicType.T2, 3, 0.15f);
            }

            Spawn();
        }

        
    }

    public void Spawn()
    {
        try
        {
            enemiesToSpawn = waves[waveNumber].StandardEnemiesToSpawn + waves[waveNumber].RangedEnemiesToSpawn;
        }

        catch
        {
            return;  //when we win the game, the WaveSceneManager will try to let spawn enemies again, so we catch the exception
        }

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
