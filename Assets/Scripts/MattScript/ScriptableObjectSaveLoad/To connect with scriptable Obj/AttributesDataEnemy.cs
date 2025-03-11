using UnityEngine;

[System.Serializable]
public class AttributesDataEnemy
{
    public int HP;
    public int ATK;
    public int DropGoal;
    public int MPDrop;
    public string SpriteName;
    public Sprite sprite;
    public ScriptableObject FavFood;

    public AttributesDataEnemy()
    {
        this.HP = 1;
        this.ATK = 1;
        this.DropGoal = 1;
        this.MPDrop = 1;
        this.SpriteName = "test1_0";
        this.sprite = null;
        this.FavFood = null;
    }
}
