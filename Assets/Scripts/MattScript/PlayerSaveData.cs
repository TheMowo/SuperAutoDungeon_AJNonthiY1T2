using System.Collections.Generic;
[System.Serializable]
public class PlayerSaveData
{
    public string uniqueID;     // Unique identifier for matching
    public float[] position;
    public int Health;
    public int Attack;
    public PlayerType playerType;
}

[System.Serializable]
public class GameSaveData
{
    public List<PlayerSaveData> players = new List<PlayerSaveData>();
}
