using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsManager : MonoBehaviour
{
    [SerializeField] Bar healthBar;
    [SerializeField] Bar speedBar;
    [SerializeField] Bar damageBar;

    // Add a player reference


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar.isFull)
        {
            // buff player
            // make PowerUp spawnable
        }
        else if (healthBar.isEmpty)
        {
            // debuff player
            // make powerUp not spawnable
        }

        if (speedBar.isFull)
        {
            // buff player
            // make PowerUp spawnable
        }
        else if (speedBar.isEmpty)
        {
            // debuff player
            // make powerUp not spawnable
        }

        if (damageBar.isFull)
        {
            // buff player
            // make PowerUp spawnable
        }
        else if (damageBar.isEmpty)
        {
            // debuff player
            // make powerUp not spawnable
        }
    }
}
