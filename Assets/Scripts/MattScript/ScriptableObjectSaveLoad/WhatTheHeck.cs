using UnityEngine;

public class WhatTheHeck : MonoBehaviour, IDataPersistence
{
    [Header("Attributes SO")]
    [SerializeField] private SomeShit SOMETHING;

    public void LoadData(GameData data)
    {
        SOMETHING.HP = data.playerAttributesData.HP;
        SOMETHING.ATK = data.playerAttributesData.ATK;
        SOMETHING.Sprite = data.playerAttributesData.sprite;
    }
    public void SaveData(ref GameData data)
    {
        data.playerAttributesData.HP = SOMETHING.HP;
        data.playerAttributesData.ATK = SOMETHING.ATK;
        data.playerAttributesData.sprite = SOMETHING.Sprite;
    }
}
