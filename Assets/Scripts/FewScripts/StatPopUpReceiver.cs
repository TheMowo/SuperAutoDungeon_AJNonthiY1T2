using UnityEngine;

public class StatPopUpReceiver : MonoBehaviour
{
    [SerializeField] private GameObject statUpPrefab;
    [SerializeField] private Canvas gameCanvas;
    
    public void ApplySOStat(ConsumableItem item)
    {
        Debug.Log("ApplySOStat() called with item: " + item.name);

        if(item.HpEffect != 0)
        {
            ShowStatPopUps(item.HpEffect, "HP");
        }
        if(item.AtkEffect != 0)
        {
            ShowStatPopUps(item.AtkEffect, "ATK");
        }
    }
    private void ShowStatPopUps(int value, string type)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-80f, 80f), Random.Range(-40f, 40f), 0);
        
        Vector3 popupPosition = transform.position + randomOffset;

        GameObject popUp = Instantiate(statUpPrefab, gameCanvas.transform);
        popUp.transform.position = popupPosition;

        StatPopUp popUpScript = popUp.GetComponent<StatPopUp>();

        popUpScript.popUpText = "";
        popUpScript.popUpColor = Color.white;

        if(value > 0)
        {
            popUpScript.popUpText = "+" + value.ToString();
            
        }
        else if(value < 0)
        {
            popUpScript.popUpText = value.ToString();
        }

        if(type == "HP")
        {
            popUpScript.popUpColor = Color.green;
        }
        else if(type == "ATK")
        {
            popUpScript.popUpColor = Color.red;
        }
    }
}
