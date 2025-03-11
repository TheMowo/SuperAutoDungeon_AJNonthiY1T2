using UnityEngine;

[CreateAssetMenu(fileName = "SomeShit", menuName = "Scriptable Objects/SomeShit", order = 1)]
public class SomeShit : ScriptableObject
{
    public int HP;
    public int ATK;
    public string SpriteName;
    public Sprite Sprite;
}
