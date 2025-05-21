using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesUnitType", menuName = "Scriptable Objects/EnemiesUnitType")]
public class EnemiesUnitType : ScriptableObject
{
    public enum EnemiesType
    {
        None,
        Undead,
        Fire,
        Steel,
        Strength
    }
    [SerializeField] public EnemiesType myType;
    public int HP;
    public int ATK;
    public int DropGold;
    public int MPDrop;
    public Sprite UnitSprite;
    public ConsumableItem FavFood;
    public bool isUndead;
    public int attackCooldown;
}
