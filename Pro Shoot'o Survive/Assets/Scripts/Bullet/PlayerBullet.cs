using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    [Header("Bullet lifetime variables")]
    [SerializeField] private float lifetime;
    private float currentLifetime;

    [Header("Homing variables")]
    public bool IsHoming;
    public Transform target;
    [SerializeField] float rotSpeed;
    [SerializeField] float homingTimer;

    protected override void Start()
    {
        currentLifetime = lifetime;
    }

    protected override void Update()
    {
        currentLifetime -= Time.deltaTime;

        if (currentLifetime <= 0)
        {
            RestoreBullet();
        }
        if (IsHoming)
        {
            homingTimer -= Time.deltaTime;
            HomingMovement();
        }
        else
        {
            StandardMovement();
        }
    }

    private void RestoreBullet()
    {
        currentLifetime = lifetime;
        WeaponManager.Instance.ReturnPlayerBullet(gameObject);

        if (IsHoming)
        {
            IsHoming = false;
            target = null;
        }
    }

    private void HomingMovement()
    {
        if (target != null)
        {
            if (homingTimer <= 0)
            {
                Vector3 distance = target.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(distance);
                Quaternion newRotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
                transform.rotation = newRotation;
            }
        }
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Standard") || other.CompareTag("Ranged"))
        {
            
            EnemyLogic enemy = other.gameObject.GetComponent<EnemyLogic>();
            enemy.ReceiveDamage(DamageDealt);
        }

        RestoreBullet();
    }
}
