using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemySaveData
{
    public string uniqueID;     // Unique identifier for matching
    public float[] position;
    public int BaseHP;
    public int BaseATK;
    public int CurrentGreyDebuff;
    public int CurrentGreenDebuff;
    public int CurrentLightBlueDebuff;
    public int CurrentGoldDebuff;
    public int LifeStealDebuff;
    public int ShieldDebuff;
    public List<DebuffEffectType> CurrentEffects;
}
