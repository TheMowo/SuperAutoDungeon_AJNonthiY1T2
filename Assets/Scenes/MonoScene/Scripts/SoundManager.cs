using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    //Converts to Singleton
    public static SoundManager Instance; private void Awake() { if (Instance != null) Destroy(this.gameObject); DontDestroyOnLoad(this.gameObject); Instance = this; }

    //AudioSources that will be spawned and destroyed to play sounds
    [SerializeField] private AudioSource _soundFxSource;
    [SerializeField] private AudioSource _uiSource;
    [SerializeField] private AudioSource _musicSource; //might move to a music manager since it uses a whole system of it's own??
    public void PlaySfxClip(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            Debug.Log("audioClip is Null");
        }
        else
        {
            //spawn in GameObject
            AudioSource _audioSource = Instantiate(_soundFxSource, new Vector2(0, 0), Quaternion.identity);
            Debug.Log("SoundManager: PlaySfxClip() has Instantiated an AudioSource at " + transform.position);

            //ensures audio does not loop
            _audioSource.loop = false;

            //assign the audioClip
            _audioSource.clip = audioClip;

            //play sound
            _audioSource.Play();

            //get length of sound fx clip
            float clipLength = _audioSource.clip.length;

            //destroy the clip after it is done playing
            Destroy(_audioSource.gameObject, clipLength);
        }
    }

    public void PlaySfxClipWithPitchChange(AudioClip audioClip)
    {

        if (audioClip == null)
        {
            Debug.Log("audioClip is Null");
        }
        else
        {
            //spawn in GameObject
            AudioSource _audioSource = Instantiate(_soundFxSource, new Vector2(0, 0), Quaternion.identity);
            Debug.Log("SoundManager: PlaySFXClipWithPitchChange() has Instantiated an AudioSource at " + transform.position);

            //ensures audio does not loop
            _audioSource.loop = false;

            _audioSource.pitch = Random.Range(0.9f, 1.5f);

            //assign the audioClip
            _audioSource.clip = audioClip;

            //play sound
            _audioSource.Play();

            //get length of sound fx clip
            float clipLength = _audioSource.clip.length;

            //destroy the clip after it is done playing
            Destroy(_audioSource.gameObject, clipLength);
        }
    }
}



[System.Serializable]
public class MusicList
{
    [SerializeField] private string _key;
    public string Key => _key;

    [SerializeField] private AudioClip _audioClip;
    public AudioClip Clip => _audioClip;
}
