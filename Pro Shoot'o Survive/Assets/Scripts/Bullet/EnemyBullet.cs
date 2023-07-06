using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void Update()
    {
        StandardMovement();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLogic player = other.gameObject.GetComponent<PlayerLogic>();
            player.TakeDamage(DamageDealt);
        }
        WeaponManager.Instance.ReturnEnemyBullet(gameObject);
    }
}
