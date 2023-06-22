using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    public float MaxHP; //Starting HP
    [HideInInspector] private float currentHP;
    [SerializeField] GameObject OwnWeapon;
    private NavMeshAgent enemyAgent;

    [SerializeField] public float meleeDamage;
    [SerializeField] Vector3 PowerUpSpawnPos;
    [SerializeField] PowerUpManager powerUpManager;
    // Start is called before the first frame update
    void Start()
    {
        powerUpManager = GameObject.FindObjectOfType<PowerUpManager>();
        currentHP = MaxHP;
        enemyAgent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void ReceiveDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            if (OwnWeapon != null)
            {
               // OwnWeapon.transform.position = new Vector3(transform.position.x, 1.5f, transform.position.y);
                OwnWeapon.transform.rotation = Quaternion.identity;
                OwnWeapon.transform.parent = null;
            }
            if (gameObject.tag == "Standard")
            {
                BarsManager.Instance.setSpeedBar(0.05f);
            }
            else if (gameObject.tag == "Ranged")
            {
                BarsManager.Instance.setDamageBar(0.05f);
            }
            WaveSceneManager wsm = FindFirstObjectByType<WaveSceneManager>();
            wsm.EnemiesSpawned.Remove(gameObject);
            PowerUpSpawnPos = transform.position;
            Destroy(gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Player"))
    //    {
    //        PlayerLogic player = collision.gameObject.GetComponent<PlayerLogic>();
    //        player.TakeDamage(meleeDamage);
    //    }
    //}

    private void OnDestroy()
    {
        int probability = Random.Range(0, 100);
        if (probability < 35)
        {
            //powerUpManager.SpawnRandomPowerUp(PowerUpSpawnPos);
            //Debug.Log(PowerUpManager.Instance);
            //PowerUpManager.Instance.SpawnRandomPowerUp(PowerUpSpawnPos);
        }
    }
}
