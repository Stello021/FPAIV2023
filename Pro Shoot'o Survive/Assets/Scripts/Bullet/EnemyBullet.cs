using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Damage of enemy bullet: " + DamageDealt);
            PlayerLogic player = other.gameObject.GetComponent<PlayerLogic>();
            player.TakeDamage(DamageDealt);
        }

        base.OnTriggerEnter(other);
    }
}
