using UnityEngine;
using UnityEngine.EventSystems;

public class NextSceneButtonSound : MonoBehaviour, IPointerDownHandler
{
    //I recommend you avert your eyes from this wacky solution to the sound problem-
    AudioClip Sfx;

    void Start()
    {
        Sfx = GameObject.Find("UiSource_OnNext").GetComponent<AudioSource>().clip;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        SoundManager.Instance.PlaySfxClipWithPitchChange(Sfx);
    }
}
