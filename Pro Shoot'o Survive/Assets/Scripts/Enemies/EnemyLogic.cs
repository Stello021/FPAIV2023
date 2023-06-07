using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] float EnemySpeed;
    [SerializeField] float MaxHP; //Starting HP
    private float currentHP;
    private NavMeshAgent enemyAgent; 
    // Start is called before the first frame update
    void Start()
    {
        currentHP = MaxHP;
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAgent.speed= EnemySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHP <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
