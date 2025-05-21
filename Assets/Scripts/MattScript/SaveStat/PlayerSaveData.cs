using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerSaveData
{
    public string uniqueID;     // Unique identifier for matching
    public float[] position;
    public int BaseHP;
    public int CurrentHP;
    public int BaseATK;
    public int CurrentATK;
    public PlayerType playerType;
    public int CurrentGreyDebuff;
    public int CurrentGreenDebuff;
    public int CurrentLightBlueDebuff;
    public int CurrentGoldDebuff;
    public List<DebuffEffectType> CurrentEffects;
}
