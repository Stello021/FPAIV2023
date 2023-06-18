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
    [HideInInspector] public SpawnerType type;

    private WaveSceneManager wsm;
    // Start is called before the first frame update
    void Start()
    {
        if (EnemiesPrefabs[0].gameObject.CompareTag("Standard"))
        {
            type = SpawnerType.Standard;
        }
        else if (EnemiesPrefabs[0].gameObject.CompareTag("Ranged"))
        {
            type = SpawnerType.Ranged;
        }
        wsm = FindFirstObjectByType<WaveSceneManager>();
    }
    public void SpawnEnemy(float speedMultiplier, float hpMultiplier)
    {
        int randomIndex = Random.Range(0, EnemiesPrefabs.Count);
        GameObject go = Instantiate(EnemiesPrefabs[randomIndex], transform.position, Quaternion.identity);
        EnemyAI e = go.GetComponent<EnemyAI>();
        EnemyLogic enemyLogic = go.GetComponent<EnemyLogic>();
        e.enemyAgent.speed = e.enemyAgent.speed * speedMultiplier;
        enemyLogic.MaxHP = enemyLogic.MaxHP * hpMultiplier;
        e.PlayerTransform = Target;
        wsm.EnemiesSpawned.Add(go);
    }
}
