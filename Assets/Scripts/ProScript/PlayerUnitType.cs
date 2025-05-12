using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUnitType", menuName = "Scriptable Objects/PlayerUnitType")]
public class PlayerUnitType : ScriptableObject
{
    public int HP;
    public int ATK;
    public Sprite UnitSprite;
}
