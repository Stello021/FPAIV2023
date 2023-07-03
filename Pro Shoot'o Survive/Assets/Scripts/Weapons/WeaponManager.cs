using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private GameObject playerBulletPrefab;

    private Queue<GameObject> enemyBulletPool = new Queue<GameObject>();
    private Queue<GameObject> playerBulletPool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.Log("WeaponManager instance already exists. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //EnemyBullet pool
        for (int i = 0; i < 50; i++)
        {
            GameObject enemyBullet = Instantiate(enemyBulletPrefab);
            enemyBullet.SetActive(false);
            enemyBulletPool.Enqueue(enemyBullet);
        }
        //PlayerBullet pool
        for (int i = 0; i < 50; i++)
        {
            GameObject playerBullet = Instantiate(playerBulletPrefab);
            playerBullet.SetActive(false);
            playerBulletPool.Enqueue(playerBullet);
        }
    }

    public GameObject GetEnemyBullet()
    {
        if (enemyBulletPool.Count > 0)
        {
            GameObject bullet = enemyBulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            Debug.LogWarning("Enemy bullet pool is empty. Creating new bullet.");
            return Instantiate(enemyBulletPrefab);
        }
    }

    public GameObject GetPlayerBullet()
    {
        if (playerBulletPool.Count > 0)
        {
            GameObject bullet = playerBulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            Debug.LogWarning("Player bullet pool is empty. Creating new bullet.");
            return Instantiate(playerBulletPrefab);
        }
    }

    public void ReturnEnemyBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        enemyBulletPool.Enqueue(bullet);
    }

    public void ReturnPlayerBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        playerBulletPool.Enqueue(bullet);
    }
}