using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer musicMixer;
    [SerializeField] TMP_Text sensitivityValue;



    public void SetVolume(float volume)
    {
        float newVolume = Mathf.Log10(volume) * 20;
        musicMixer.SetFloat("MusicVolume", newVolume);
    }

    public void SetCameraSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        sensitivityValue.text = sensitivity.ToString("0.0");
    }
}
