using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerSaveData
{
    public string uniqueID;     // Unique identifier for matching
    public float[] position;
    public int Health;
    public int Attack;
    public PlayerType playerType;
}
