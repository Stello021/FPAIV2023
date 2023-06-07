using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    [SerializeField] private Transform destinantionTransform; //enemy destination transform
    [SerializeField] private Animator enemyAnimator;
    private NavMeshAgent enemyAgent;

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
        enemyAgent.destination = destinantionTransform.position;
    }
}
