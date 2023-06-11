using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Grenade : MonoBehaviour
{
    [SerializeField] float throwForceValue;
    [SerializeField] float explosionDelay;
    [SerializeField] float timeToExplosion;
    [SerializeField] float explosionRadius;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] float explosionForce;
    [SerializeField] Rigidbody rb;

    public GameObject explosionFX;
    // Start is called before the first frame update
    void Start()
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
        GameObject go = Instantiate(explosionFX, transform.position, transform.rotation);

        Collider[] explodingColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyMask);
        Debug.Log(explodingColliders.Length);
        foreach (Collider collider in explodingColliders)
        {
            EnemyAI enemy = collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.DisableAgent();
                enemy.rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, -1);
                Debug.Log("Esploso");
            }
            
            //EnemyLogic enemy = collider.GetComponent<EnemyLogic>();
            //enemyMask.currentHP--;
            
        }

        Destroy(gameObject);
        Destroy(go, 3f);
    }

    public void Throw(Vector3 dir)
    {
        Debug.Log(throwForceValue);
        rb.AddForce(dir * throwForceValue * Time.deltaTime, ForceMode.Impulse);
    }
}