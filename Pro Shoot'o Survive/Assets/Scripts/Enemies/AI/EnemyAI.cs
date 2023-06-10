using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform PlayerTransform; //enemy destination transform
    protected Animator enemyAnimator;
    protected NavMeshAgent enemyAgent;
    public Rigidbody rb;
    protected float reactivationTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        enemyAnimator.speed = enemyAgent.speed;
    }
    // Update is called once per frame
    void Update()
    {
        if (enemyAgent.isOnNavMesh)
        {
            enemyAgent.SetDestination(PlayerTransform.position); 
        }
    }

    public void DisableAgent()
    {
        rb.isKinematic = false;
        enemyAgent.enabled = false;
        enemyAnimator.enabled = false;
        Invoke("EnableAgent", reactivationTime);
    }

    public void EnableAgent()
    {
        enemyAgent.enabled = true;
        rb.isKinematic = true;
        enemyAnimator.enabled = true;
    }
}
