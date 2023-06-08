using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAI : EnemyAI
{
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] GameObject bulletPrefab;
    public Transform BulletStartingPoint;
    private float startingEnemySpeed;

    //Shooting
    public float TimeBetweenShoots;
    private bool alreadyShooted;
    public float ShootRange;
    private bool playerInShootRange;

    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAgent.stoppingDistance = ShootRange;
        startingEnemySpeed = enemyAgent.speed;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Check for shoot range
        playerInShootRange = Physics.CheckSphere(transform.position, ShootRange, whatIsPlayer);
        if (!playerInShootRange)
        {
            enemyAgent.speed = startingEnemySpeed;
            enemyAnimator.SetInteger("WeaponType_int", 0);
            enemyAnimator.SetBool("Shoot_b", false);
            enemyAgent.SetDestination(PlayerTransform.position);

        }
        else
        {
            ShootPlayer();
        }
        enemyAnimator.SetFloat("Speed_f", enemyAgent.speed);

    }
    private void ShootPlayer()
    {
        enemyAgent.SetDestination(transform.position);
        enemyAgent.speed = 0;
        if (!alreadyShooted)
        {
            enemyAnimator.SetInteger("WeaponType_int", 1);
            enemyAnimator.SetBool("Shoot_b", true);
            //Instantiate(bulletPrefab, transform.position + Vector3.forward, Quaternion.identity).GetComponent<Rigidbody>();
            alreadyShooted = true;
            Invoke(nameof(ResetAttack), TimeBetweenShoots);
        }
    }
    private void ResetAttack()
    {
        alreadyShooted = false;
    }
    private void Animations()
    {
    }
}
