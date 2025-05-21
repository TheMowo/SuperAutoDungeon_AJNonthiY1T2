using UnityEngine;
using UnityEngine.EventSystems;

public class StartCombatButtonSound : MonoBehaviour, IPointerDownHandler
{
    //I recommend you avert your eyes from this wacky solution to the sound problem-
    AudioClip Sfx;
    SoundManager soundManager;

    void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
        Sfx = GameObject.Find("UiSource_OnStart").GetComponent<AudioSource>().clip;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        soundManager.PlaySfxClipWithPitchChange(Sfx);
    }
}
