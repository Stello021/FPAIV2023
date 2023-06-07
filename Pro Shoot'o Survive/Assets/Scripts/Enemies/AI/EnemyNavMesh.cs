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
    }
    // Update is called once per frame
    void Update()
    {
        enemyAnimator.speed = enemyAgent.speed;
        enemyAgent.destination = destinantionTransform.position;
    }
}
