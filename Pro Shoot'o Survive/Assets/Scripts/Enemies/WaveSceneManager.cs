using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSceneManager : MonoBehaviour
{
    private int waveNumber = 1;
    private Spawner[] spawners;
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
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].SpawnEnemies(waveNumber);
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
