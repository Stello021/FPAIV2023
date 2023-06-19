using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsManager : MonoBehaviour
{
    static private BarsManager instance;
    static public BarsManager Instance { get { return instance; } }

    //[SerializeField] Bar healthBar;
    [SerializeField] Bar speedBar;
    [SerializeField] Bar damageBar;

    [SerializeField] Bar HpBar;

    public PlayerLogic playerRef;
    
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        HpBar.setFullBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRef != null)
        {
            playerRef.speedBarValue = speedBar.amount;
            //playerRef.healthBarValue = healthBar.amount;
            playerRef.damageBarValue = damageBar.amount;
        }
    }

    public void setHpBar(float fillAmount)
    {
        HpBar.amount = fillAmount;
    }
}
