using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float bulletSpeed;
    public Vector3 dir;

    [HideInInspector] public float DamageDealt;

    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    protected void StandardMovement()
    {
        transform.position += dir * bulletSpeed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
    }
}
