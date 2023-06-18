using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    static private PlayerLogic instance;
    static public PlayerLogic Instance { get { return instance; } }

    [SerializeField] float hpMax;
    [HideInInspector] public float actuallyMaxHp;
    [HideInInspector] public float hp;

    [SerializeField] float damageMax;
    [HideInInspector] public float damage;

    [SerializeField] float speedMax;
    [HideInInspector] public float speed;
    
    public float speedBarValue;     //value from 0 to 1
    public float healthBarValue;    //value from 0 to 1
    public float damageBarValue;    //value from 0 to 1

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        actuallyMaxHp = hpMax;
        //BarsManager.Instance.playerRef = this;
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        
        if (hp <= 0)
        {

            //player is dead
        
        }
        
        BarsManager.Instance.setHpBar(hp / actuallyMaxHp);
    }

    public void Heal(float HealAmount)
    {
        hp = Mathf.Clamp(hp + healthBarValue, 0, actuallyMaxHp);
        BarsManager.Instance.setHpBar(hp / actuallyMaxHp);
    }

}
