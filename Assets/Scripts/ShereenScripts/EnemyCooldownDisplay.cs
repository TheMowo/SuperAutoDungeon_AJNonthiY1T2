using TMPro;
using UnityEngine;

public class EnemyCooldownDisplay : MonoBehaviour
{
    [SerializeField] public TMP_Text cooldownText;

    public EnemiesUnit myUnit;
    public int currentCooldown;

    void Awake()
    {
        myUnit = GetComponent<EnemiesUnit>();
        currentCooldown = myUnit.enemiesUnitType.attackCooldown;
        UpdateText();
    }

    public bool TickDownCooldown()
    {
        currentCooldown--;
        if (currentCooldown <= 0)
        {
            Debug.Log("Cooldown is 0, returning true");
            currentCooldown = myUnit.enemiesUnitType.attackCooldown;
            UpdateText();
            return true;
        }
        Debug.Log("Cooldown is not 0, returning false");
        UpdateText();
        return false;
    }


    private void UpdateText()
    {
        if (cooldownText != null)
            cooldownText.text = $"{currentCooldown}";
    }
}
