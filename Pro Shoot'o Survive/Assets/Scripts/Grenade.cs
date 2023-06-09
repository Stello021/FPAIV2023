using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Grenade : MonoBehaviour
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
        //rb.AddForce(forceDir * forceValue * Time.deltaTime, ForceMode.Impulse);
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
        GameObject go = Instantiate(explosionFX, transform.position, transform.rotation);

        Collider[] explodingColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyMask);

        foreach (Collider collider in explodingColliders)
        {
            Rigidbody enemyRB = collider.GetComponent<Rigidbody>();
            NavMeshAgent enemy = collider.GetComponent<NavMeshAgent>();
            enemy.enabled = false;
            
            
            enemyRB.AddExplosionForce(explosionForce, transform.position, explosionRadius, 2, ForceMode.Impulse); 
            
            //EnemyLogic enemy = collider.GetComponent<EnemyLogic>();
            //enemyMask.currentHP--;
            enemy.enabled = true;
            
            Destroy(enemyRB);
        }

        //Destroy(go);
        Destroy(gameObject);
    }

    public void Throw(Vector3 dir)
    {
        rb.AddForce(dir * forceValue * Time.deltaTime, ForceMode.Impulse);
    }
}
//commento