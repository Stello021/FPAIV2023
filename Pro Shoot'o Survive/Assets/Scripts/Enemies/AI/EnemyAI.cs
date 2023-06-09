using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform PlayerTransform; //enemy destination transform
    protected Animator enemyAnimator;
    public NavMeshAgent enemyAgent;
    public Rigidbody rb;
    protected float reactivationTime = 2f;
    public bool IsAlive = true;


    [SerializeField] public float meleeDamage;
    private float meleeElapsedTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyAgent.isOnNavMesh && PlayerTransform != null)
        {
            if (IsAlive)
            {
                enemyAnimator.SetFloat("Speed_f", enemyAgent.speed);//Associate animator's speed parameter with agent speed
                enemyAgent.SetDestination(PlayerTransform.position);
                float targetDistance = Vector3.Distance(transform.position, PlayerTransform.position);
                if (targetDistance <= 3f)
                {
                    float timer = 1f;
                    meleeElapsedTime += Time.deltaTime;
                    if (meleeElapsedTime >= timer)
                    {
                        PlayerTransform.GetComponent<PlayerLogic>().TakeDamage(meleeDamage);
                        meleeElapsedTime = 0f;
                    }
                }
            }
            else
            {
                enemyAgent.SetDestination(transform.position);
            }
        }
    }

    public void DisableAgent()
    {
        rb.isKinematic = false;
        enemyAgent.enabled = false;
        enemyAnimator.enabled = false;
        Invoke(nameof(EnableAgent), reactivationTime);
    }

    public void EnableAgent()
    {
        try
        {
            enemyAgent.enabled = true;
            rb.isKinematic = true;
            enemyAnimator.enabled = true;
        }

        catch //in case the enemy has been killed, we'll catch the exception
        {
        }
    }

}
