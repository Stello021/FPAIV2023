using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    public float MaxHP; //Starting HP
    [HideInInspector] private float currentHP;
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

    }

    public void ReceiveDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            if (OwnWeapon != null)
            {
                OwnWeapon.transform.parent = null;
                OwnWeapon.transform.position = new Vector3(transform.position.x, 1.5f, transform.position.y);
                OwnWeapon.transform.rotation = Quaternion.identity;
            }
            if (gameObject.tag == "Standard")
            {
                BarsManager.Instance.setSpeedBar(0.05f);
            }
            else if (gameObject.tag == "Ranged")
            {
                BarsManager.Instance.setDamageBar(0.05f);
            }

            Destroy(gameObject);
        }
    }
}
