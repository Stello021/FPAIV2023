using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public Vector3 forceDir;
    public float forceValue;
    public float explosionDelay;
    [SerializeField]
    private float timeToExplosion;
    public float explosionRadius;
    public LayerMask enemyMask;
    public float explosionForce;
    public Rigidbody rb;

    public GameObject explosionFX;
    // Start is called before the first frame update
    void Start()
    {
        timeToExplosion = explosionDelay;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(forceDir * forceValue * Time.deltaTime, ForceMode.Impulse);
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
            Rigidbody rigidBody = collider.GetComponent<Rigidbody>();
            //EnemyLogic enemy = collider.GetComponent<EnemyLogic>();
            //enemyMask.currentHP--;
            rigidBody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            
        }

        Destroy(gameObject);
    }

    public void Throw(Vector3 dir)
    {
        rb.AddForce(dir * forceValue * Time.deltaTime, ForceMode.Impulse);
    }
}
