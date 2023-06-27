using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Grenade : MonoBehaviour
{
    [SerializeField] float throwForceValue;
    [SerializeField] float explosionDelay;
    private float timeToExplosion;
    [SerializeField] float explosionRadius;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] float explosionForce;
    private Rigidbody rb;

    [SerializeField] float damage;

    public GameObject explosionFX;
    // Start is called before the first frame update
    void Awake()
    {
        timeToExplosion = explosionDelay;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        timeToExplosion -= Time.deltaTime;

        if (timeToExplosion <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(explosionFX, transform.position, transform.rotation);

        Collider[] explodingColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyMask);

        foreach (Collider collider in explodingColliders)
        {
            EnemyAI enemy = collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.DisableAgent();
                enemy.rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                collider.GetComponent<EnemyLogic>().ReceiveDamage(damage);
            }
            
        }

        Destroy(gameObject);
    }

    public void Throw(Vector3 dir)
    {
        rb.AddForce(dir * throwForceValue, ForceMode.Impulse);
    }
}