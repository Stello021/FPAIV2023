using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    [Header("Homing variables")]
    [SerializeField] bool IsHoming;
    public Transform target;
    [SerializeField] float rotSpeed;
    [SerializeField] float speedBeforeHoming;
    [SerializeField] float normalSpeed;

    protected override void FixedUpdate()
    {
        if (IsHoming) HomingMovement();
        else StandardMovement();
    }

    private void HomingMovement()
    {
        if (target != null)
        {
            Vector3 distance = target.position - transform.position;
            Quaternion rotationDir = Quaternion.LookRotation(distance);
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, rotationDir, rotSpeed * Time.deltaTime);
            rb.MoveRotation(newRotation);
            rb.velocity = transform.forward * bulletSpeed * Time.fixedDeltaTime;

            //transform.rotation = newRotation;
            //transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        }
        else
        {
            StandardMovement();
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Standard") || collision.collider.CompareTag("Ranged"))
        {
            //enemy damage
            EnemyLogic enemy = collision.collider.gameObject.GetComponent<EnemyLogic>();
            enemy.ReceiveDamage(DamageDealt);
            Debug.Log("Danni al nemico:" + DamageDealt);
        }

        base.OnCollisionEnter(collision);
    }


    public IEnumerator WaitToEnableHoming()
    {
        bulletSpeed = speedBeforeHoming;
        dir = transform.forward;
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        IsHoming = true;
        bulletSpeed = normalSpeed;
    }

}
