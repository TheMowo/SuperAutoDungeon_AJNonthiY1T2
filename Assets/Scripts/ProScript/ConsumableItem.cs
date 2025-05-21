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
        Shield,
        LifeStealPotion,
        FragileStrengthPotion
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
    public int LifeStealDebuffEffect;
    public int ShieldDebuffEffect;
    public int duration;
    public int price;
}
