using System.Collections;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    static private AudioManager instance;
    static public AudioManager Instance { get { return instance; } }

    [SerializeField] MusicManager musicMngr;
    [SerializeField] private AudioMixer mixer;
    

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        musicMngr.MusicStart();
    }

    public void ChangeMusic(MusicType newType, float fadeDuration = 3, float targetVolume = 1)
    {
        musicMngr.ChangeMusic(newType, fadeDuration, targetVolume);
    }

    private void OnDestroy()
    {
        float volumeOff = Mathf.Log10(0.0001f) * 20;
        mixer.SetFloat(GetMixerParamName(MusicType.first), volumeOff);
        mixer.SetFloat(GetMixerParamName(MusicType.second), volumeOff);
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
            case MusicType.first:
                return "T1_Vol";
            case MusicType.second:
                return "T2_Vol";
        }

        return result;
    }
}
