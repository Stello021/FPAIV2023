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
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] Slider volumeSlider;


    private void Start()
    {
        float volumeValue;
        musicMixer.GetFloat("MusicVolume", out volumeValue);
        volumeValue = Mathf.Pow(10, volumeValue / 20);
        Debug.Log(volumeValue);
        volumeSlider.value = volumeValue;

        Debug.Log(PlayerPrefs.GetFloat("Sensitivity"));
        if (PlayerPrefs.GetFloat("Sensitivity") != 0)
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
    }

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
