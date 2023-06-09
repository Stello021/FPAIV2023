using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> EnemiesPrefabs;
    public Transform Target;
    private List<GameObject> EnemiesSpawned;
    // Start is called before the first frame update
    void Start()
    {
        EnemiesSpawned= new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies(int waveNumber)
    {
        StartCoroutine(SpawnRoutine(waveNumber));
    }

    public void DestroyEnemies()
    {
        for (int i = 0; i < EnemiesSpawned.Count; i++)
        {
            Destroy(EnemiesSpawned[i]);
        }
    }

    IEnumerator SpawnRoutine(int waveNumber)
    {
        for (int i = 0; i < waveNumber; i++)
        {
            int randomIndex = Random.Range(0, EnemiesPrefabs.Count);
            GameObject go = Instantiate(EnemiesPrefabs[randomIndex], transform.position, Quaternion.identity);
            EnemyAI e = go.GetComponent<EnemyAI>();
            e.PlayerTransform = Target;
            EnemiesSpawned.Add(go);
            yield return new WaitForSeconds(5f);
        }
    }
}
