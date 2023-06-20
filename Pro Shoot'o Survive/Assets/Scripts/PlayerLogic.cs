using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    static private PlayerLogic instance;
    static public PlayerLogic Instance { get { return instance; } }

    [SerializeField] float hpMax;
    [HideInInspector] public float actuallyMaxHp;

    [SerializeField] private float hp;
    public float HP { get { return hp; } set { hp = Mathf.Clamp(hp + value, 0, hpMax); } }
    [SerializeField] private float armor = 0;
    public float Armor { get { return armor; } set { armor += value; } }

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
        hp = actuallyMaxHp;
        BarsManager.Instance.playerRef = this;
    }

    // Update is called once per frame
    void Update()
    {
        BarsManager.Instance.setHpBar(hp / actuallyMaxHp);
    }

    public void TakeDamage(float damage)
    {
        if (armor > 0)
        {
            armor -= damage;
        }
        else
        {
            hp -= damage;
        }

        Debug.Log("Hp: " + hp);

        if (hp <= 0)
        {
            Destroy(gameObject);
        }

    }

    public void Heal(float HealAmount)
    {
        hp = Mathf.Clamp(hp + healthBarValue, 0, actuallyMaxHp);
        BarsManager.Instance.setHpBar(hp / actuallyMaxHp);
    }

}
