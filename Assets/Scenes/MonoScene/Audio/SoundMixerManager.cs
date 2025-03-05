using UnityEngine;
using UnityEngine.Audio;
public class SoundMixerManager : MonoBehaviour
{
    //Converts to Singleton
    public static SoundMixerManager Instance; private void Awake() { if (Instance != null) Destroy(this.gameObject); DontDestroyOnLoad(this.gameObject); Instance = this; }

    [SerializeField] private AudioMixer _audioMixer;

    public void SetMasterVolume(float level)
    {
        _audioMixer.SetFloat("masterVolume", level);
    }

    public void SetSFXVolume(float level)
    {
        _audioMixer.SetFloat("sfxVolume", level);
    }

    public void SetMusicVolume(float level)
    {
        _audioMixer.SetFloat("musicVolume", level);
    }
}
