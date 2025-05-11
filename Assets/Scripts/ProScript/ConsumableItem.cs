using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "Scriptable Objects/ConsumableItem")]
public class ConsumableItem : ScriptableObject
{
    public enum ItemEffectType
    {
        Food,
        HealingPotion,
        InstantDamage,
        StatsPotion,
        CleansingPotion,
        BetterCleansingPotion
    }
    [SerializeField] public ItemEffectType myEffectType;
    public string Name;
    public string Description;
    public int HpEffect;
    public int AtkEffect;
    public Sprite Sprite;
    public int GreenDebuffEffect;
    public int GreyDebuffEffect;
    public int LightBlueDebuffEffect;
    public int GoldDebuffEffect;
    public int BlackDebuffEffect;
    public int price;
}
