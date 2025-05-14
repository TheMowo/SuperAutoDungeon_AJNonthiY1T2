using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ShopSaveSystem : MonoBehaviour
{
    public List<ShopSlot> ShopSlot;
    private string ItemSavePath => Path.Combine(Application.persistentDataPath, "ShopSaveData.json");
    public string fileName;
    private FileDataHandler dataHandler;
    public GameObject ItemPrefab;
    public GameObject Shop;
    private int I;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Keeps this GameObject across scenes
    }

    void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); //Application.persistentDataPath <== change this to save where ever you want
        Debug.Log(this.dataHandler);
        if (GameObject.Find("Player Unit 1") == true)
        {
            ShopLoad();
        }
    }

    private void Update()
    {
        if (GameObject.Find("Player Unit 1") == true)
        {
            if (Shop.active == true && I < 0.1f)
            {
                GetAllShopSlotList();
                ShopLoad();
                I += 1;
            }
        }
    }

    public void GetAllShopSlotList()
    {
        ShopSlot = FindObjectsByType<ShopSlot>(FindObjectsSortMode.None).ToList();
    }

    public void ShopoSaveData()
    {
        var saveData = new GameSaveData();
        foreach (var Shopitem in ShopSlot)
        {
            saveData.Shopitems.Add(Shopitem.GetDataSave());
        }

        string json4 = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(ItemSavePath, json4);
        Debug.Log("Shop Saved!");
    }

    public void ShopLoad()
    {
        if (!File.Exists(ItemSavePath))
        {
            Debug.LogWarning("No save file found. Clearing all inventory slots.");
            return;
        }

        string json4 = File.ReadAllText(ItemSavePath);
        Debug.Log("Loaded JSON:\n" + json4);

        var saveData = JsonUtility.FromJson<GameSaveData>(json4);

        // Create dictionary for faster slot lookup
        Dictionary<int, ShopSaveData> slotDataDict = saveData.Shopitems.ToDictionary(item => item.SlotIndex, item => item);

        foreach (var slot in ShopSlot)
        {
            int slotIndex = slot.transform.GetSiblingIndex();

            if (slotDataDict.TryGetValue(slotIndex, out var slotData) && slotData.item != null)
            {
                // Slot has saved data
                bool hasChild = slot.transform.childCount > 0;

                if (hasChild)
                {
                    Debug.Log($"Loading data into existing item in slot {slotIndex}");
                    slot.LoadFromSaveData(slotData);
                }
                else
                {
                    // Create new child and load data
                    Debug.Log($"Creating new item in slot {slotIndex}");
                    GameObject newItem = Instantiate(ItemPrefab, slot.transform);
                    newItem.transform.localPosition = Vector3.zero;
                    slot.LoadFromSaveData(slotData);
                }
            }
            else
            {
                // Slot has no saved data
                bool hasChild = slot.transform.childCount > 0;

                if (hasChild)
                {
                    // Remove existing child
                    Debug.Log($"Clearing slot {slotIndex} with no saved data");
                    ClearSlotChildren(slot.gameObject);
                }
                // Else: Do nothing (no saved data and no child)
            }
        }
        Debug.Log("Shop Loaded!");
    }

    private void ClearAllInventorySlots()
    {
        foreach (var slot in ShopSlot)
        {
            ClearSlotChildren(slot.gameObject);
        }
    }

    private void ClearSlotChildren(GameObject slot)
    {
        // Destroy all children of the slot
        foreach (Transform child in slot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void DeleteSaveFile()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, "ShopSaveData.json");
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Debug.Log("Save file deleted: " + fullPath);
        }
        else
        {
            Debug.LogWarning("No save file found to delete at: " + fullPath);
        }
    }
}
