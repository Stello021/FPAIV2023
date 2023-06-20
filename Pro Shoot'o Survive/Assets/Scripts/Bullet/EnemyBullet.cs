using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    [SerializeField] float enemyDamage;

    protected override void Start()
    {
        base.Start();
        DamageDealt = enemyDamage;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("Damage of enemy bullet: " + DamageDealt);
            PlayerLogic player = collision.gameObject.GetComponent<PlayerLogic>();
            player.TakeDamage(DamageDealt);
        }

        base.OnCollisionEnter(collision);
    }
}
