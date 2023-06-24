using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum MusicType { T1, T2, }

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource[] musicSources;
    [SerializeField] MusicType[] sourcesTypes;
    [SerializeField] MusicType startType;
    [SerializeField] float fadeInTime;


    private Dictionary<MusicType, AudioSource> mSources;
    private MusicType currentType;

    private void Awake()
    {
        mSources = new Dictionary<MusicType, AudioSource>();
        for (int i = 0; i < musicSources.Length; i++)
        {
            mSources.Add(sourcesTypes[i], musicSources[i]);
        }
    }

    public void MusicStart()
    {
        currentType = startType;

        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i].Play();
        }

        StartCoroutine(AudioManager.Instance.AdjustVolumeRoutine(startType, fadeInTime, 1));
    }

    public void ChangeMusic(MusicType newType, float fadeDuration)
    {
        if (newType != currentType)
        {
            StartCoroutine(AudioManager.Instance.AdjustVolumeRoutine(currentType, fadeDuration, 0));
            StartCoroutine(AudioManager.Instance.AdjustVolumeRoutine(newType, fadeDuration, 1));
            currentType = newType;
        }
    }
}
