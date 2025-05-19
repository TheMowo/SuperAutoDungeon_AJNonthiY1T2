using TMPro;
using UnityEngine;

public class EnemyCooldownDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text cooldownText;
    [SerializeField] private TMP_Text targetText;

    private EnemiesUnit myUnit;
    private int currentCooldown;

    void Awake()
    {
        myUnit = GetComponent<EnemiesUnit>();
        currentCooldown = myUnit.enemiesUnitType.attackCooldown;
        UpdateText();
    }

    public void TickDownCooldown()
    {
        currentCooldown--;
        if (currentCooldown <= 0)
        {
            currentCooldown = myUnit.enemiesUnitType.attackCooldown;
        }
        UpdateText();
    }

    public void SetTargetText(string target)
    {
        if (targetText != null)
            targetText.text = target;
    }

    private void UpdateText()
    {
        if (cooldownText != null)
            cooldownText.text = $"CD: {currentCooldown}";
    }
}
