using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    static private AudioManager instance;
    static public AudioManager Instance { get { return instance; } }

    [SerializeField] MusicManager musicMngr;
    [SerializeField] private AudioMixer mixer;

    [SerializeField] private AudioMixerSnapshot mainSnap;
    [SerializeField] private AudioMixerSnapshot pauseSnap;

    bool isInPause;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        musicMngr.MusicStart();
    }

    public void ChangeMusic(MusicType newType, float fadeDuration, float targetVolume)
    {
        musicMngr.ChangeMusic(newType, fadeDuration, targetVolume);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isInPause)
            {
                OnPause(isInPause);
                isInPause = true;
            }
            else
            {
                OnPause(isInPause);
                isInPause = false;
            }
        }
    }

    private void OnPause(bool pause)
    {
        if (pause)
        {
            pauseSnap.TransitionTo(0.2f);
        }
        else
        {
            mainSnap.TransitionTo(0.2f);
        }
    }

    public void FadeIn(MusicType type, float duration, float targetVolume, float startDelay = 0)
    {

    }

    public IEnumerator FadeInRoutine(AudioSource source, float duration, float targetVolume, float startDelay = 0)
    {
        yield return new WaitForSeconds(startDelay);

        float currentTime = 0;
        float currentVolume = source.volume = 0;
        source.Play();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float newVolume = Mathf.Lerp(currentVolume, targetVolume, currentTime / duration);
            source.volume = newVolume;
            yield return null; // ci fa attendere il frame successivo, senza di questo currentTime arriverebbe 
                               // subito al valore duration
        }

        source.volume = targetVolume;


        yield break;
    }

    public IEnumerator FadeOutRoutine(AudioSource source, float duration, float startDelay = 0)
    {
        yield return new WaitForSeconds(startDelay);

        float currentTime = 0;
        float currentVolume = source.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float newVolume = Mathf.Lerp(currentVolume, 0, currentTime / duration);

            source.volume = newVolume;

            yield return null;
        }

        source.volume = 0;
        source.Stop();

        yield break;
    }

    public IEnumerator AdjustVolumeRoutine(MusicType type, float duration, float targetVolume, float startDelay = 0)
    {
        yield return new WaitForSeconds(startDelay);

        string paramName = GetMixerParamName(type);

        targetVolume = Mathf.Clamp(targetVolume, 0.0001f, 1);

        float currentTime = 0;
        float currentVolume = 0;
        mixer.GetFloat(paramName, out currentVolume);
        currentVolume = Mathf.Pow(10, currentVolume / 20);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float newVolume = Mathf.Lerp(currentVolume, targetVolume, currentTime / duration);
            newVolume = Mathf.Log10(newVolume) * 20;
            mixer.SetFloat(paramName, newVolume);

            yield return null;
        }

        mixer.SetFloat(paramName, Mathf.Log10(targetVolume) * 20);

        yield break;
    }


    private string GetMixerParamName(MusicType type)
    {
        string result = "";

        switch (type)
        {
            case MusicType.T1:
                return "T1_Vol";
            case MusicType.T2:
                return "T2_Vol";
        }

        return result;
    }
}
