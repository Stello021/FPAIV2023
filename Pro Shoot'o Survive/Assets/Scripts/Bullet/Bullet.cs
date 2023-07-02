using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float bulletSpeed;
    [SerializeField] public Vector3 dir;

    public float DamageDealt;

    [SerializeField] protected float lifetime;

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected void StandardMovement()
    {
        transform.position += dir * bulletSpeed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {

    }
}
