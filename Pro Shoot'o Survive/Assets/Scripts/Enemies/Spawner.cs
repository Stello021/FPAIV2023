using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnerType
{
    Standard, Ranged
}
public class Spawner : MonoBehaviour
{
    public List<GameObject> EnemiesPrefabs;
    public Transform Target;
    private List<GameObject> EnemiesSpawned;
    public SpawnerType type;
    // Start is called before the first frame update
    void Start()
    {
        EnemiesSpawned= new List<GameObject>();
        if (EnemiesPrefabs[0].gameObject.CompareTag("Standard"))
        {
            type = SpawnerType.Standard;
        }
        else if (EnemiesPrefabs[0].gameObject.CompareTag("Ranged"))
        {
            type = SpawnerType.Ranged;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, EnemiesPrefabs.Count);
        GameObject go = Instantiate(EnemiesPrefabs[randomIndex], transform.position, Quaternion.identity);
        EnemyAI e = go.GetComponent<EnemyAI>();
        e.PlayerTransform = Target;
        EnemiesSpawned.Add(go);
        //StartCoroutine(SpawnRoutine(enemiesToSpawn));
    }

    public void DestroyEnemies()
    {
        for (int i = 0; i < EnemiesSpawned.Count; i++)
        {
            Destroy(EnemiesSpawned[i]);
        }
    }

    IEnumerator SpawnRoutine(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
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
