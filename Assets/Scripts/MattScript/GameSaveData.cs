using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public List<PlayerSaveData> players = new List<PlayerSaveData>();
    public List<ItemSaveData> items = new List<ItemSaveData>();
    public List<EnemySaveData> Enemys = new List<EnemySaveData>();
}