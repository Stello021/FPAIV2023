using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] float TotalValue;
    [SerializeField] float StartingValue;
    [SerializeField] float decreaseValuePerSecond;

    float value;
    float time;

    public float amount { get { return bar.fillAmount; } set { bar.fillAmount = value; } }

    public bool isFull { get { return isFull; } set { value = bar.fillAmount >= 1; } }
    public bool isEmpty { get { return isEmpty; } set { value = bar.fillAmount <= 0; } }

    // Start is called before the first frame update
    void Start()
    {
        value = StartingValue;
        bar.fillAmount = value / TotalValue;

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void DecreaseinTime()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            AddAmount(-decreaseValuePerSecond);
            time = 1f;
        }
    }
    public void AddAmount(float amount)
    {
        value += amount;
        bar.fillAmount = value / TotalValue;
    }

    public void setFullBar()
    {
        value = TotalValue;
        bar.fillAmount = value / TotalValue;

    }

    public void setEmptyBar()
    {
        value = 0;
        bar.fillAmount = value / TotalValue;

    }
}
