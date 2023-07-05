using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI_Controller : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        
        Time.timeScale = 0;
    }

    public void StartPlaying()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;

        Destroy(gameObject);  //destroys tutorial UI panel
    }
}
