using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class AttributesData
{
    public int HP;
    public int ATK;
    public string SpriteName;
    public Sprite sprite;

    public AttributesData()
    {
        this.HP = 1;
        this.ATK = 1;
        this.SpriteName = "test1_0";
        this.sprite = Resources.Load<Sprite>(SpriteName);
    }
}
