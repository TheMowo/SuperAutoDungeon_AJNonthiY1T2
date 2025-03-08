using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesUnitType", menuName = "Scriptable Objects/EnemiesUnitType")]
public class EnemiesUnitType : ScriptableObject
{
    public int HP;
    public int ATK;
    public int DropGold;
    public int MPDrop;
    public Sprite Sprite;
    public ConsumableItem FavFood;
}
