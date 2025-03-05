using UnityEngine;

public class Health_ForTest : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (this == null)
        {
            return;
        }

        currentHealth -= damage;
        Debug.Log(gameObject.name + " HP: " + currentHealth);

        if (IsDead())
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " is dead");
        Destroy(gameObject);
    }
}

