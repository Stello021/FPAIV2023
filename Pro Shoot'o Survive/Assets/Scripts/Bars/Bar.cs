using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] float TotalValue;
    [SerializeField] float decreaseValuePerSecond;

    float value;
    float time;

    public bool isFull { get { return isFull; } set { value = bar.fillAmount >= 1; } }
    public bool isEmpty { get { return isEmpty; } set { value = bar.fillAmount <= 0; } }

    // Start is called before the first frame update
    void Start()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            AddAmount(-decreaseValuePerSecond);
            time = 1f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddAmount(float amount)
    {
        value += amount;
        bar.fillAmount = value / TotalValue;
    }
}
