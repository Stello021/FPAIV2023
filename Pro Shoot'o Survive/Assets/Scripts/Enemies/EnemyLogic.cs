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
    [SerializeField] GameObject bloodVFX;
    private Transform center;
    public Transform Center { get { return center; } }
    private PlayerLogic player;
    private int pointsValue = 100;

    // Start is called before the first frame update
    void Start()
    {
        Transform playerTransform = GetComponent<EnemyAI>().PlayerTransform;
        player = playerTransform.GetComponent<PlayerLogic>();
        center = transform.GetChild(2);
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
        BloodVFXManager.Instance.GetBloodVFX(Center.position);
        //Instantiate(bloodVFX, Center.position, Quaternion.identity);
        //Debug.Log("Current HP: " + currentHP);
        if (currentHP <= 0)
        {
            player.AddPoints(pointsValue);
            StartCoroutine(Death());
        }
    }

    public void UpdatePoints(int points)
    {
        pointsValue += points;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }



    public IEnumerator Death()
    {
        Animator enemyAnimator = GetComponent<Animator>();
        enemyAnimator.SetBool("Death_b", true);
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
            BarsManager.Instance.setDamageBar(-0.10f);

        }
        else if (gameObject.tag == "Ranged")
        {
            BarsManager.Instance.setDamageBar(0.15f);
            BarsManager.Instance.setSpeedBar(-0.10f);

        }
        WaveSceneManager wsm = FindFirstObjectByType<WaveSceneManager>();
        wsm.EnemiesSpawned.Remove(gameObject);

        PowerUpSpawnPos = Center.position;

        Destroy(center.gameObject);
        Collider c = GetComponent<Collider>();
        Destroy(c);
        yield return new WaitForSeconds(3f);

        // check if we must spawn a random powerUp
        int probability = Random.Range(0, 100);
        if (probability < powerUpProbability)
        {
            PowerUpManager.Instance.SpawnRandomPowerUp(PowerUpSpawnPos);
        }

        DestroyEnemy();
        yield return null;


    }

}
