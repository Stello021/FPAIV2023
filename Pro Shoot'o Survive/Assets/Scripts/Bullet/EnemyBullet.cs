using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            PlayerLogic player = collision.gameObject.GetComponent<PlayerLogic>();
            player.TakeDamage(DamageDealt);
        }

        base.OnCollisionEnter(collision);
    }
}
