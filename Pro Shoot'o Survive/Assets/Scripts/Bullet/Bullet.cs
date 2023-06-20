using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float bulletSpeed;
    [SerializeField] public Vector3 dir;

    public float DamageDealt;

    [SerializeField] protected Rigidbody rb;

    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        StandardMovement();
    }

    protected void StandardMovement()
    {
        rb.velocity = dir * bulletSpeed * Time.fixedDeltaTime;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
