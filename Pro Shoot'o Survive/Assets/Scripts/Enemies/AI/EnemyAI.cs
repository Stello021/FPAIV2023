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
    protected float reactivationTime = 3f;

    [SerializeField] public float meleeDamage;

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
        enemyAnimator.SetFloat("Speed_f", enemyAgent.speed);//Associate animator's speed parameter with agent speed
        if (enemyAgent.isOnNavMesh && PlayerTransform != null)
        {
            enemyAgent.SetDestination(PlayerTransform.position);
            Debug.Log(enemyAgent.destination);
            if (enemyAgent.remainingDistance <= 0.01f)
            {
                PlayerTransform.GetComponent<PlayerLogic>().TakeDamage(meleeDamage);
            }
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
