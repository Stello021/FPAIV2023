using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer musicMixer;

    public void SetVolume(float volume)
    {
        float newVolume = Mathf.Log10(volume) * 20;
        musicMixer.SetFloat("MusicVolume", newVolume);
    }
}
