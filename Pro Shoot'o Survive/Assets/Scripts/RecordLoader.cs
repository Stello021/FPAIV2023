using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecordLoader : MonoBehaviour
{
    [SerializeField] TMP_Text recordValueText;
    // Start is called before the first frame update
    void Awake()
    {
        recordValueText.text = PlayerPrefs.GetInt("Current").ToString();
    }
}
