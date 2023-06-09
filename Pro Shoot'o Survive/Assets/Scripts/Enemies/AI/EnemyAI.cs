using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform PlayerTransform; //enemy destination transform
    protected Animator enemyAnimator;
    protected NavMeshAgent enemyAgent;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAnimator.speed = enemyAgent.speed;
    }
    // Update is called once per frame
    void Update()
    {
        enemyAgent.SetDestination(PlayerTransform.position);
    }
}
