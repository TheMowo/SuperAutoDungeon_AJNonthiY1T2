using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "Scriptable Objects/ConsumableItem")]
public class ConsumableItem : ScriptableObject
{
    public string Name;
    public string Description;
    public int HpEffect;
    public int AtkEffect;
    public Sprite Sprite;
    public int GreenDebuffEffect;
    public int RedDebuffEffect;
    public int BlueDebuffEffect;
}
