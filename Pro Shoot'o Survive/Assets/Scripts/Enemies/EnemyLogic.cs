using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] float MaxHP; //Starting HP
    [HideInInspector]public float currentHP;
    [SerializeField] GameObject OwnWeapon;
    private NavMeshAgent enemyAgent; 
    // Start is called before the first frame update
    void Start()
    {
        currentHP = MaxHP;
        enemyAgent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        if(currentHP <= 0) 
        {
            OwnWeapon.transform.parent= null;
            OwnWeapon.transform.position =new Vector3(transform.position.x , 1.5f, transform.position.y);
            OwnWeapon.transform.rotation = Quaternion.identity;
            Destroy(gameObject);
        }
    }
}
