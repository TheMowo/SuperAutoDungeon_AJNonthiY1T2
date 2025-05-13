using UnityEngine;
using UnityEngine.Audio;
public class SoundMixerManager : MonoBehaviour
{
    //Converts to Singleton
    public static SoundMixerManager Instance; private void Awake() { if (Instance != null) Destroy(this.gameObject); DontDestroyOnLoad(this.gameObject); Instance = this; }

    [SerializeField] private AudioMixer _audioMixer;

    public void SetMasterVolume(float level)
    {
        _audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSFXVolume(float level)
    {
        _audioMixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        _audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }
}
