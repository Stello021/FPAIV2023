using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    static private PlayerLogic instance;
    static public PlayerLogic Instance { get { return instance; } }

    [SerializeField] float hpMax;
    float hp;
    
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
        BarsManager.Instance.playerRef = this;
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
        
        BarsManager.Instance.setHpBar(hp / hpMax);
    }

    public void Heal(float HealAmount)
    {
        hp = Mathf.Clamp(hp + healthBarValue, 0, hpMax);
        BarsManager.Instance.setHpBar(hp / hpMax);
    }
}
