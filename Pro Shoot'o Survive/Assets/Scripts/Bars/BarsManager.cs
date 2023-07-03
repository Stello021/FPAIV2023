using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsManager : MonoBehaviour
{
    static private BarsManager instance;
    static public BarsManager Instance { get { return instance; } }

    //[SerializeField] Bar healthBar;
    [SerializeField] Bar speedBar;
    [SerializeField] Bar ReloadBar;

    [SerializeField] Bar HpBar;

    public PlayerLogic playerRef;
    
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerRef != null)
        {
            playerRef.speedBarValue = speedBar.amount;
            playerRef.ReloadBarValue = ReloadBar.amount;
        }
    }

    public void setHpBar(float fillAmount)
    {
        HpBar.amount = fillAmount;
    }
    public void setSpeedBar(float fillAmount)
    {
        speedBar.amount += fillAmount;
    }
    public void setDamageBar(float fillAmount)
    {
        ReloadBar.amount += fillAmount;
    }
}
