using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void Update()
    {
        base.Update();

        StandardMovement();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Damage of enemy bullet: " + DamageDealt);
            PlayerLogic player = other.gameObject.GetComponent<PlayerLogic>();
            player.TakeDamage(DamageDealt);
        }

        base.OnTriggerEnter(other);
    }
    
}
