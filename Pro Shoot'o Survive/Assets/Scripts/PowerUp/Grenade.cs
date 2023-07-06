using UnityEngine;

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
            EnemyLogic enemy = collider.GetComponent<EnemyLogic>();
            enemy.ReceiveDamage(damage);

            EnemyAI enemyAI = collider.GetComponent<EnemyAI>();
            if (enemyAI.IsAlive)
            {
                enemyAI.DisableAgent();
                enemyAI.rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
            
        }

        Destroy(gameObject);
    }

    public void Throw(Vector3 dir)
    {
        rb.AddForce(dir * throwForceValue, ForceMode.Impulse);
    }
}