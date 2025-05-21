using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSound : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //Highly unoptimized script tbh
    AudioSource PickUpSfx;
    AudioSource DropSfx;
    SoundManager soundManager;

    void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
        PickUpSfx = GameObject.Find("SfxSource_ItemPickUp").GetComponent<AudioSource>();
        DropSfx = GameObject.Find("SfxSource_ItemDrop").GetComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        soundManager.PlaySfxClipWithPitchChange(PickUpSfx.clip);
    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        soundManager.PlaySfxClipWithPitchChange(DropSfx.clip);
    }
}
