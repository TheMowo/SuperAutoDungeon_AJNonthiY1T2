using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    //List Of Audio
    [SerializeField] private List<SfxList> _sfxList;

    public void PlaySfx(string key)
    {
        foreach (var sound in _sfxList)
        {
            if (sound.Key == key)
            {
                // play sfx
                SoundManager.Instance.PlaySfxClip(sound.Clip);
                return;
            }
        }
    }
    public void PlaySfxWithRandomPitch(string key)
    {
        foreach (var sound in _sfxList)
        {
            if (sound.Key == key)
            {
                // play sfx
                SoundManager.Instance.PlaySfxClipWithPitchChange(sound.Clip);
                return;
            }
        }
    }

}

[System.Serializable]
public class SfxEntry
{
    [SerializeField] private string _key;
    public string Key => _key;

    [SerializeField] private AudioClip _clip;
    public AudioClip Clip => _clip;
}

[System.Serializable]
public class SfxList
{
    [SerializeField] private string _key;
    public string Key => _key;

    [SerializeField] private AudioClip _audioClip;
    public AudioClip Clip => _audioClip;
}
