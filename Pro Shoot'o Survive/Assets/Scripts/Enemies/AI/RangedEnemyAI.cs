using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class RangedEnemyAI : EnemyAI
{
    [SerializeField] LayerMask PlayerMask; //Layer mask to recognize Player Layer
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject OwnWeapon;
    public Transform BulletStartingPoint; //Bullets Spawn Point
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
        startingEnemySpeed = enemyAgent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for shoot range, range is the ray of sphere with origin on enemy position 
        playerInShootRange = Physics.CheckSphere(transform.position, ShootRange, PlayerMask);
        enemyAnimator.SetFloat("Speed_f", enemyAgent.speed);//Associate animator's speed parameter with agent speed
        //FSM
        if (!playerInShootRange)
        {
            ChasePlayer();
        }
        else
        {
            ShootPlayer();
        }

    }
    private void ChasePlayer()
    {
        OwnWeapon.SetActive(false); //Deactivate Weapon
        enemyAgent.speed = startingEnemySpeed; 
        enemyAgent.SetDestination(PlayerTransform.position);
        //Animation settings
        enemyAnimator.SetInteger("WeaponType_int", 0);
        enemyAnimator.SetBool("Shoot_b", false);

    }
    private void ShootPlayer()
    {
        gameObject.transform.LookAt(new Vector3(PlayerTransform.position.x, 0, PlayerTransform.position.z));
        enemyAgent.speed = 0;
        enemyAgent.SetDestination(transform.position);
        if (!alreadyShooted)
        {
            //Animator settings
            enemyAnimator.SetInteger("WeaponType_int", 1);
            enemyAnimator.SetBool("Shoot_b", true);
            OwnWeapon.SetActive(true);
            Invoke(nameof(SpawnBullet), TimeBetweenShoots);
            alreadyShooted = true;
            Invoke(nameof(ResetAttack), TimeBetweenShoots);
            
        }
    }
    private void SpawnBullet()
    {
        Instantiate(bulletPrefab, BulletStartingPoint.position, BulletStartingPoint.rotation);
    }
    private void ResetAttack()
    {
        alreadyShooted = false;
    }

}
