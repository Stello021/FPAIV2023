using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PointsLoader : MonoBehaviour
{
    [SerializeField] TMP_Text recordValueText;
    // Start is called before the first frame update
    void Awake()
    {
        recordValueText.text = PlayerPrefs.GetInt("Record").ToString();
    }

}
