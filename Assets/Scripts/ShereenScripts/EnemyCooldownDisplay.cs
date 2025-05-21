using TMPro;
using UnityEngine;

public class EnemyCooldownDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text cooldownText;

    private EnemiesUnit myUnit;
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
            currentCooldown = myUnit.enemiesUnitType.attackCooldown;
            UpdateText();
            return true;
        }
        UpdateText();
        return false;
    }


    private void UpdateText()
    {
        if (cooldownText != null)
            cooldownText.text = $"{currentCooldown}";
    }
}
