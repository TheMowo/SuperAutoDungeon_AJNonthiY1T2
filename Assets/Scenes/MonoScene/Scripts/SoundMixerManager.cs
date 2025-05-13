using UnityEngine;
using UnityEngine.Audio;
public class SoundMixerManager : MonoBehaviour
{
    //Converts to Singleton
    public static SoundMixerManager Instance; private void Awake() { if (Instance != null) Destroy(this.gameObject); DontDestroyOnLoad(this.gameObject); Instance = this; }

    [SerializeField] private AudioMixer _audioMixer;

    public void SetMasterVolume(float level)
    {
        _audioMixer.SetFloat("MasterVolume", level);
        //_audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSFXVolume(float level)
    {
        _audioMixer.SetFloat("SfxVolume", level);
        //_audioMixer.SetFloat("SfxVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        _audioMixer.SetFloat("MusicVolume", level);
        //_audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }

    public void SetUiVolume(float level)
    {
        _audioMixer.SetFloat("UiVolume", level);
        //_audioMixer.SetFloat("UiVolume", Mathf.Log10(level) * 20f);
    }
}
