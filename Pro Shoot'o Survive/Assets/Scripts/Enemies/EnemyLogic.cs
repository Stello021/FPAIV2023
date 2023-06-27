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
    [SerializeField] float powerUpProbability;

    // Start is called before the first frame update
    void Start()
    {
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
        //Debug.Log("Current HP: " + currentHP);
        if (currentHP <= 0)
        {

            StartCoroutine(Death());

        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }


    public IEnumerator Death()
    {
        EnemyAI e = GetComponent<EnemyAI>();
        e.IsAlive = false;
        if (OwnWeapon != null)
        {
            OwnWeapon.transform.rotation = Quaternion.identity;
            OwnWeapon.transform.parent = null;
            RotatingManager.Instance.AddRotatingObject(OwnWeapon.transform);
        }
        if (gameObject.tag == "Standard")
        {
            BarsManager.Instance.setSpeedBar(0.15f);
            BarsManager.Instance.setDamageBar(-0.15f);

        }
        else if (gameObject.tag == "Ranged")
        {
            BarsManager.Instance.setDamageBar(0.15f);
            BarsManager.Instance.setSpeedBar(-0.15f);

        }
        WaveSceneManager wsm = FindFirstObjectByType<WaveSceneManager>();
        wsm.EnemiesSpawned.Remove(gameObject);
        Animator enemyAnimator = GetComponent<Animator>();
        enemyAnimator.SetBool("Death_b", true);
        Rigidbody rb = GetComponent<Rigidbody>();

        PowerUpSpawnPos = transform.GetChild(2).position;
        int probability = Random.Range(0, 100);
        if (probability < powerUpProbability)
        {
            PowerUpManager.Instance.SpawnRandomPowerUp(PowerUpSpawnPos);
        }

        Destroy(rb);
        Collider c = GetComponent<Collider>();
        Destroy(c);
        yield return new WaitForSeconds(3f);
        DestroyEnemy();
        yield return null;


    }

}
