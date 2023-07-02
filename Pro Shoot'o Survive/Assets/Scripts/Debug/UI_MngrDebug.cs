using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_MngrDebug : Singleton<UI_MngrDebug>
{
    public List<string> TextSpritesKeys;
    public List<TextMeshProUGUI> TextSpritesValues;
    public Dictionary<string, TextMeshProUGUI> TextSprites;

    // Start is called before the first frame update
    void Start()
    {
        TextSprites = new Dictionary<string, TextMeshProUGUI>();

        for (int i = 0; i < TextSpritesKeys.Count; i++)
        {
            TextSprites[TextSpritesKeys[i]] = TextSpritesValues[i];
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
