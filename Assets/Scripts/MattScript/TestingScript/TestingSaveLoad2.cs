using UnityEngine;

public class TestingSaveLoad2 : MonoBehaviour, IDataPersistence
{
    [Header("Item ID")]
    [SerializeField] private string id;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data)
    {
        
    }
    public void SaveData(ref GameData data)
    {
        
    }
}
