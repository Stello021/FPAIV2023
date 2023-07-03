using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float reloadTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (image.fillAmount < 1)
        {
            image.fillAmount += 1f / reloadTime * Time.deltaTime;

            if (image.fillAmount >= 1)
            {
                gameObject.SetActive(false);
                image.fillAmount = 0;
            }
        }
    }
}
